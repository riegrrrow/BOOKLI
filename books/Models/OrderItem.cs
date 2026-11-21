namespace books.Models;

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }

    public string BookTitle { get; set; } = "";

    public string Author { get; set; } = "";

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public string? Image { get; set; }

    public Order? Order { get; set; }
}
