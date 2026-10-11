namespace ePizzaHub.UI.Models.ApiModels.Response
{
    public class ValidateUserResponse
    {
        public string AccessToken { get; set; }
        public int TokenExpiryInMinutes { get; set; }
    }
}
