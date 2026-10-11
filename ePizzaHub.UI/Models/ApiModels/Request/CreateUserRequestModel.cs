namespace ePizzaHub.UI.Models.ApiModels.Request
{
    public class CreateUserRequestModel
    {
        public string Name { get; set; } = default!;
        public string Email { get; set; } = default!;
        public string Password { get; set; } = default!;
        public string PhoneNumber { get; set; } = default!;
    }
}
