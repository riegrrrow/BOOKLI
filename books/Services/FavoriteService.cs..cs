
using books.Data;
using books.Models;
using Microsoft.EntityFrameworkCore;

namespace books.Services;

public class FavoriteService
{
    private readonly ApplicationDbContext db;

    public FavoriteService(ApplicationDbContext context)
    {
        db = context;
    }

    // Получение избранных книг
    public async Task<List<Book>> GetFavoritesAsync(string userId)
    {
        var favorites = await db.FavoriteBooks
            .Where(x => x.UserId == userId)
            .ToListAsync();

        var allBooks = await db.Books.ToListAsync();
        var result = new List<Book>();
        bool changed = false;

        foreach (var favorite in favorites)
        {
            Book? book;

            // Проверяем, сохранён ли уже числовой ID
            if (int.TryParse(favorite.BookId, out int id))
            {
                book = allBooks.FirstOrDefault(x => x.Id == id);
            }
            else
            {
                // Поддержка старых избранных, сохранённых по названию
                book = allBooks.FirstOrDefault(
                    x => x.Title == favorite.BookId);
            }

            if (book == null)
                continue;

            // Переводим старое название в числовой ID
            string correctId = book.Id.ToString();

            if (favorite.BookId != correctId)
            {
                favorite.BookId = correctId;
                changed = true;
            }

            result.Add(book);
        }

        if (changed)
            await db.SaveChangesAsync();

        return result;
    }

    public async Task<bool> IsFavoriteAsync(string userId, string bookId)
    {
        return await db.FavoriteBooks.AnyAsync(
            x => x.UserId == userId && x.BookId == bookId);
    }

    public async Task ToggleFavoriteAsync(string userId, string bookId)
    {
        var favorite = await db.FavoriteBooks.FirstOrDefaultAsync(
            x => x.UserId == userId && x.BookId == bookId);

        if (favorite != null)
        {
            db.FavoriteBooks.Remove(favorite);
        }
        else
        {
            db.FavoriteBooks.Add(new FavoriteBook
            {
                UserId = userId,
                BookId = bookId
            });
        }

        await db.SaveChangesAsync();
    }
}
