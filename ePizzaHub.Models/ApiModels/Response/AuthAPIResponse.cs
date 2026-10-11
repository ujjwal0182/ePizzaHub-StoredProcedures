
namespace ePizzaHub.Models.ApiModels.Response
{
    public class AuthAPIResponse
    {
        public string AccessToken { get; set; }
        public int TokenExpiryInMinutes { get; set; }
    }
}
