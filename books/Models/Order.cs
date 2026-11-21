namespace books.Models;

public class Order
{
    public int Id { get; set; }

    public string UserId { get; set; } = "";

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public decimal Total { get; set; }

    public string Status { get; set; } = "Новый";

    public string PaymentMethod { get; set; } = "При получении";

    public string PaymentStatus { get; set; } = "Ожидает оплаты";

    public string DeliveryAddress { get; set; } = "";

    public List<OrderItem> Items { get; set; } = new();
}
