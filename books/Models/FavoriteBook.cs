namespace books.Models;

public class FavoriteBook
{
    public int Id { get; set; }

    public string UserId { get; set; } = "";

    public string BookId { get; set; } = "";
}