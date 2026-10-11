namespace ePizzaHub.UI.Models.ApiModels.Response
{
    public class ItemResponseModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal UnitPrice { get; set; }

        public string ImageUrl { get; set; }
    }
}
