namespace ePizzaHub.UI.Helpers
{
    public interface ITokenService
    {
        void SetToken(string token); //save token in cookies 
        string GetToken(); //get token from cookies
    }
}
