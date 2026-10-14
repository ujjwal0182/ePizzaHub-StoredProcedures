using ePizzaHub.Mcp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();
builder.Services.AddTransient<ForwardAuthHandler>();

builder.Services.AddHttpClient<PizzaApiClient>(c =>
    c.BaseAddress = new Uri(builder.Configuration["EPizzaAPI:Url"]!)
).AddHttpMessageHandler<ForwardAuthHandler>();

builder.Services.AddMcpServer()
    .WithHttpTransport(o => o.Stateless = true)   // har request independent: headers har baar naye
    .WithToolsFromAssembly();                     // [McpServerToolType] wali classes khud mil jaati hain

var app = builder.Build();

app.MapMcp("/mcp");

app.Run();
