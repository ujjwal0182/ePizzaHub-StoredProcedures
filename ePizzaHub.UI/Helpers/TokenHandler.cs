///// <summary>
/// The purpose of this class to remove the client.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenService.GetToken()}"); from every API call 
/// in the controller and move it to a single place (TokenHandler) and this will automatically add the token in the header of every API call, 
/// so we don't have to worry about adding the token in every API call in the controller.
/// 
/// This is basically a interceptor which will intercept the API call and add the token in the header of every API call, so we don't have to worry about adding the token in every API call in the controller.
///// </summary>

namespace ePizzaHub.UI.Helpers
{
    public class TokenHandler : DelegatingHandler
    {
        private readonly ITokenService _tokenService;

        public TokenHandler(ITokenService tokenService)
        {
            this._tokenService = tokenService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var token = _tokenService.GetToken();
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
