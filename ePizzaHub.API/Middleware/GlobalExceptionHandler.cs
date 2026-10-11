using ePizzaHub.Core.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace ePizzaHub.API.Middleware
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        // SP me jo THROW number diye the unka HTTP status
        private static readonly Dictionary<int, int> SqlErrorMap = new()
        {
            [50001] = StatusCodes.Status409Conflict,            // email already registered
            [50010] = StatusCodes.Status400BadRequest,          // invalid quantity
            [50011] = StatusCodes.Status404NotFound,            // item not found
            [50012] = StatusCodes.Status409Conflict,            // cart inactive
            [50013] = StatusCodes.Status404NotFound,            // cart not found
            [50014] = StatusCodes.Status404NotFound,            // cart item not found
            [50020] = StatusCodes.Status404NotFound,            // active cart not found for user
            [50021] = StatusCodes.Status400BadRequest,          // cart empty
            [50022] = StatusCodes.Status422UnprocessableEntity  // paid amount mismatch
        };

        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IHostEnvironment _env;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger, IHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext ctx, Exception ex, CancellationToken ct)
        {
            ProblemDetails problem;

            if (ex is SqlException sql && SqlErrorMap.TryGetValue(sql.Number, out var status))
            {
                _logger.LogWarning("Business rule violation {Number}: {Message}", sql.Number, sql.Message);
                problem = new ProblemDetails { Status = status, Title = "Request could not be processed", Detail = sql.Message };
            }
            else if (ex is SqlException { Number: 2601 or 2627 })
            {
                // Unique index/constraint violation (do requests ek saath same email register karein)
                _logger.LogWarning(ex, "Duplicate key");
                problem = new ProblemDetails { Status = 409, Title = "Duplicate record." };
            }
            else if (ex is BusinessRuleException)
            {
                problem = new ProblemDetails { Status = 422, Title = "Request could not be processed", Detail = ex.Message };
            }
            else if (ex is HttpRequestException)
            {
                _logger.LogError(ex, "Payment gateway call failed");
                problem = new ProblemDetails { Status = 502, Title = "Payment gateway is unavailable. Please try again." };
            }
            else if (ex is UnauthorizedAccessException)
            {
                problem = new ProblemDetails { Status = 401, Title = "Unauthorized", Detail = ex.Message };
            }
            else
            {
                _logger.LogError(ex, "Unhandled exception");
                problem = new ProblemDetails
                {
                    Status = 500,
                    Title = "Something went wrong.",
                    Detail = _env.IsDevelopment() ? ex.Message : null
                };
            }

            ctx.Response.StatusCode = problem.Status!.Value;
            await ctx.Response.WriteAsJsonAsync(problem, ct);
            return true;
        }
    }
}
