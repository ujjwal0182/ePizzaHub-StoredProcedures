namespace ePizzaHub.UI.Models.ViewModels
{
    public class UserModel
    {
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; } = "9999999999"; //Should be real customer number, but for testing purpose we are using dummy number
    }
}
