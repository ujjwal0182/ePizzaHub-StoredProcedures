namespace ePizzaHub.Mcp.Models
{
    public record MenuItem (int Id, string Name, string Description, decimal UnitPrice);
    public record CartLine(int ItemId, string ItemName, decimal UnitPrice, int Quantity);
    public record CartView(decimal Total, decimal Tax, decimal GrandTotal, List<CartLine> Items);
    public record OrderView(string Id, DateTime CreatedDate, decimal GrandTotal, string Status);
}
