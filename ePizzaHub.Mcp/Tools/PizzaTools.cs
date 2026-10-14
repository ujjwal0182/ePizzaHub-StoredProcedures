using ePizzaHub.Mcp.Models;
using ePizzaHub.Mcp.Services;
using ModelContextProtocol;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace ePizzaHub.Mcp.Tools
{
    [McpServerToolType]
    public static class PizzaTools
    {
        [McpServerTool(ReadOnly = true)]
        [Description("Search the pizza menu. Returns id, name, description and price in INR. Leave the query empty to list all items.")]
        public static async Task<List<MenuItem>> SearchMenu(
            PizzaApiClient api,
            [Description("Optional keyword such as 'paneer' or 'cheese'")] string query = "")
        {
            var menu = await api.GetMenuAsync();
            if (string.IsNullOrWhiteSpace(query)) return menu;

            return menu.Where(i =>
                    i.Name.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    i.Description.Contains(query, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        [McpServerTool(ReadOnly = true)]
        [Description("Show the current cart with line items, total, tax and grand total in INR.")]
        public static async Task<CartView> GetCart(PizzaApiClient api) => await api.GetCartAsync();


        [McpServerTool]
        [Description("Add a menu item to the current cart. Use SearchMenu first to get a valid itemId.")]
        public static async Task<CartView> AddToCart(
            PizzaApiClient api,
            [Description("Menu Item id from search menu")] int itemId,
            [Description("How many to add from 1 to 10")] int quantity)
        {
            if (quantity is < 1 or > 10)
                throw new McpException("Quantiry must be between 1 to 10");

            await api.AddToCartAsync(itemId, quantity);
            return await api.GetCartAsync();
        }

        [McpServerTool(Destructive = true)]
        [Description("Remove an item completely from the current cart.")]
        public static async Task<CartView>RemoveFromCart(PizzaApiClient api, [Description("Menu Item id to remove")] int itemId)
        {
            await api.RemoveFromCartAsync(itemId);
            return await api.GetCartAsync();
        }

        [McpServerTool(ReadOnly = true)]
        [Description("List the logged-in user's past orders with date, total and status. Requires the user to be logged in.")]
        public static Task<List<OrderView>> GetMyOrders(PizzaApiClient api) => api.GetOrdersAsync();
    }
}
