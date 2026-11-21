using books.Data;
using books.Models;
using Microsoft.EntityFrameworkCore;

namespace books.Services;

public class BookService
{
    private readonly ApplicationDbContext db;

    public BookService(ApplicationDbContext context)
    {
        db = context;
    }

    // Получение всех книг
    public async Task<List<Book>> GetBooksAsync()
    {
        return await db.Books
            .OrderBy(x => x.Id)
            .ToListAsync();
    }

    // Получение одной книги
    public async Task<Book?> GetBookAsync(int id)
    {
        return await db.Books.FindAsync(id);
    }

    // Добавление книги
    public async Task AddBookAsync(Book book)
    {
        db.Books.Add(book);
        await db.SaveChangesAsync();
    }

    // Изменение книги
    public async Task UpdateBookAsync(Book book)
    {
        var existingBook = await db.Books.FindAsync(book.Id);

        if (existingBook == null)
            return;

        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.Genre = book.Genre;
        existingBook.Price = book.Price;
        existingBook.Color = book.Color;
        existingBook.Emoji = book.Emoji;
        existingBook.Description = book.Description;
        existingBook.Image = book.Image;
        existingBook.Cover = book.Cover;

        // Дополнительные характеристики
        existingBook.PageCount = book.PageCount;
        existingBook.PublicationYear = book.PublicationYear;
        existingBook.CoverType = book.CoverType;
        existingBook.Rating = book.Rating;

        await db.SaveChangesAsync();
    }
    // Удаление книги
    public async Task DeleteBookAsync(int id)
    {
        var book = await db.Books.FindAsync(id);

        if (book != null)
        {
            db.Books.Remove(book);
            await db.SaveChangesAsync();
        }
    }
}