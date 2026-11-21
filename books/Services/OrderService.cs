
using books.Data;
using books.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace books.Services
{
    public class OrderService
    {
        private readonly ApplicationDbContext db;

        public OrderService(ApplicationDbContext context)
        {
            db = context;
        }

        // Заказы конкретного пользователя
        public async Task<List<Order>> GetOrdersAsync(string userId)
        {
            return await db.Orders
                .Include(x => x.Items)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        // Все заказы для администратора
        public async Task<List<Order>> GetOrdersAsyncForAdmin()
        {
            return await db.Orders
                .Include(x => x.Items)
                .OrderByDescending(x => x.CreatedAt)
                .ToListAsync();
        }

        // Создание заказа
        public async Task<Order> CreateOrderAsync(
            string userId,
            string address,
            List<OrderItem> items,
            string paymentMethod)
        {
            if (string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("Укажите адрес доставки.");

            if (items == null || items.Count == 0)
                throw new ArgumentException("Корзина пуста.");

            if (items.Any(x => x.Quantity <= 0 || x.UnitPrice < 0))
                throw new ArgumentException("Некорректные данные товаров.");

            if (paymentMethod != "При получении" &&
                paymentMethod != "Онлайн")
                throw new ArgumentException("Выбран неизвестный способ оплаты.");

            var order = new Order
            {
                UserId = userId,
                CreatedAt = DateTime.UtcNow,
                DeliveryAddress = address,
                Status = "Новый",

                PaymentMethod = paymentMethod,

                PaymentStatus = paymentMethod == "Онлайн"
                    ? "Ожидает оплаты"
                    : "Оплата при получении",

                Items = items.Select(x => new OrderItem
                {
                    BookTitle = x.BookTitle,
                    Author = x.Author,
                    UnitPrice = x.UnitPrice,
                    Quantity = x.Quantity,
                    Image = x.Image
                }).ToList()
            };

            order.Total = order.Items.Sum(x => x.UnitPrice * x.Quantity);

            db.Orders.Add(order);
            await db.SaveChangesAsync();

            return order;
        }

        // Изменение статуса заказа
        public async Task UpdateOrderStatusAsync(int orderId, string newStatus)
        {
            var allowedStatuses = new[]
            {
                "Новый",
                "В обработке",
                "Отправлен",
                "Доставлен",
                "Отменён"
            };

            if (!allowedStatuses.Contains(newStatus))
                throw new ArgumentException("Недопустимый статус заказа.");

            var order = await db.Orders.FindAsync(orderId);

            if (order == null)
                throw new Exception("Заказ не найден.");

            order.Status = newStatus;

            await db.SaveChangesAsync();
        }
        // Получение подробностей заказа конкретного пользователя
        public async Task<Order?> GetOrderByIdAsync(int orderId, string userId)
        {
            return await db.Orders
                .Include(x => x.Items)
                .FirstOrDefaultAsync(x => x.Id == orderId && x.UserId == userId);
        }
    }
}

