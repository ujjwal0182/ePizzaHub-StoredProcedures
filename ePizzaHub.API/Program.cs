using ePizzaHub.API.Middleware;
using ePizzaHub.API.Services;
using ePizzaHub.API.Settings;
using ePizzaHub.Core.Concrete;
using ePizzaHub.Core.Contracts;
using ePizzaHub.Core.Settings;
using ePizzaHub.Repositories.Concrete;
using ePizzaHub.Repositories.Contracts;
using ePizzaHub.Repositories.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Threading.RateLimiting;
using OllamaSharp;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((ctx, cfg) => cfg
    .ReadFrom.Configuration(ctx.Configuration)
    .Enrich.FromLogContext());

var connectionString = builder.Configuration.GetConnectionString("Default")
    ?? throw new InvalidOperationException("ConnectionStrings:Default is missing.");

var jwt = builder.Configuration.GetSection("Jwt").Get<JwtSettings>()
    ?? throw new InvalidOperationException("Jwt section is missing.");
if (string.IsNullOrWhiteSpace(jwt.Secret) || jwt.Secret.Length < 32)
    throw new InvalidOperationException("Jwt:Secret must be at least 32 characters (set it in user-secrets).");

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<PricingSettings>(builder.Configuration.GetSection("Pricing"));
builder.Services.Configure<RazorpaySettings>(builder.Configuration.GetSection("Razorpay"));
builder.Services.Configure<AssistantSettings>(builder.Configuration.GetSection("Assistant"));

builder.Services.AddHttpClient<IPaymentGateway, RazorpayGateway>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(o =>
{
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Secret)),
            ClockSkew = TimeSpan.Zero
        };
    });
builder.Services.AddAuthorization();

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

builder.Services.AddSingleton<IChatClient>(sp =>
{
    var s = sp.GetRequiredService<IOptions<AssistantSettings>>().Value;
    var http = new HttpClient { BaseAddress = new Uri(s.OllamaUrl), Timeout = TimeSpan.FromMinutes(5) };
    return new ChatClientBuilder(new OllamaApiClient(http, s.Model))
        .UseFunctionInvocation(null, c => c.MaximumIterationsPerRequest = s.MaxIterations)
        .Build();
});

builder.Services.AddRateLimiter(o =>
{
    o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    o.AddPolicy("assistant", ctx =>
    {
        var key = ctx.User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                  ?? ctx.Connection.RemoteIpAddress?.ToString() ?? "anon";
        return RateLimitPartition.GetConcurrencyLimiter(key, _ => new ConcurrencyLimiterOptions
        {
            PermitLimit = 1,
            QueueLimit = 0
        });
    });
});

// Data access
builder.Services.AddSingleton<IDbConnectionFactory>(new SqlConnectionFactory(connectionString));
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

// Services
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenGeneratorService, TokenGeneratorService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IAssistantService, AssistantService>();

var app = builder.Build();

app.UseExceptionHandler();
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();     // pehle authentication
app.UseRateLimiter();
app.UseAuthorization();      // phir authorization
app.MapControllers();
app.Run();