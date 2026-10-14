using ModelContextProtocol.Protocol;

namespace ePizzaHub.Mcp.Services
{
    public class ForwardAuthHandler : DelegatingHandler
    {
        private readonly IHttpContextAccessor _accessor;
        public ForwardAuthHandler(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            var auth = _accessor.HttpContext?.Request.Headers.Authorization.ToString();
            if (!string.IsNullOrEmpty(auth))
                request.Headers.TryAddWithoutValidation("Authorization", auth);

            return base.SendAsync(request, ct);
        }
    }
}
