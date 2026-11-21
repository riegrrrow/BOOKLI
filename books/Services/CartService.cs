namespace books.Services;
public class CartService
{
    public class CartItem
    {
        public string Title { get; set; } = "";
        public string Author { get; set; } = "";
        public decimal Price { get; set; }
        public string Image { get; set; } = "";
        public int Quantity { get; set; } = 1;
    }

    public List<CartItem> Items { get; private set; } = new();

    public event Action? OnChange;

    // Добавление книги
    public void Add(string title, string author, decimal price, string image)
    {
        var existing = Items.FirstOrDefault(x => x.Title == title);

        if (existing != null)
        {
            existing.Quantity++;
        }
        else
        {
            Items.Add(new CartItem
            {
                Title = title,
                Author = author,
                Price = price,
                Image = image,
                Quantity = 1
            });
        }

        NotifyStateChanged();
    }

    // Увеличение количества
    public void Increase(CartItem item)
    {
        item.Quantity++;
        NotifyStateChanged();
    }

    // Уменьшение количества
    public void Decrease(CartItem item)
    {
        if (item.Quantity > 1)
        {
            item.Quantity--;
        }
        else
        {
            Items.Remove(item);
        }

        NotifyStateChanged();
    }

    // Удаление книги
    public void Remove(CartItem item)
    {
        Items.Remove(item);
        NotifyStateChanged();
    }

    // Очистка корзины
    public void Clear()
    {
        Items.Clear();
        NotifyStateChanged();
    }

    // Общая стоимость
    public decimal TotalPrice =>
        Items.Sum(x => x.Price * x.Quantity);

    // Общее количество книг
    public int TotalCount =>
        Items.Sum(x => x.Quantity);
    

    private void NotifyStateChanged()
    {
        OnChange?.Invoke();
    }
}