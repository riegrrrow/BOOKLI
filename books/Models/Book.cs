namespace books.Models;

public class Book
{
    public int Id { get; set; }

    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string Genre { get; set; } = "";

    public decimal Price { get; set; }

    public string Color { get; set; } = "";
    public string Emoji { get; set; } = "📚";

    public string Description { get; set; } = "";

    public string Image { get; set; } = "";
    public string Cover { get; set; } = "";

    // Дополнительная информация о книге
    public int PageCount { get; set; }

    public int PublicationYear { get; set; }

    public string CoverType { get; set; } = "Твёрдая";

    // Средний рейтинг книги
    public decimal Rating { get; set; } = 0;

    public Book()
    {
    }

    public Book(
        string title,
        string author,
        string genre,
        decimal price,
        string color,
        string emoji,
        string description,
        string image)
    {
        Title = title;
        Author = author;
        Genre = genre;
        Price = price;
        Color = color;
        Emoji = emoji;
        Description = description;
        Image = image;
    }
}