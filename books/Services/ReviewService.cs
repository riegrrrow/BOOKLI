using books.Data;
using books.Models;
using Microsoft.EntityFrameworkCore;

namespace books.Services;

public class ReviewService
{
    private readonly ApplicationDbContext db;

    public ReviewService(ApplicationDbContext context)
    {
        db = context;
    }

    // Получение отзывов о книге
    public async Task<List<BookReview>> GetReviewsAsync(int bookId)
    {
        return await db.BookReviews
            .Where(x => x.BookId == bookId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }

    // Добавление или изменение отзыва
    public async Task AddReviewAsync(
        int bookId,
        string userId,
        string userName,
        int rating,
        string comment)
    {
        if (rating < 1 || rating > 5)
            throw new ArgumentException("Оценка должна быть от 1 до 5.");

        if (string.IsNullOrWhiteSpace(comment))
            throw new ArgumentException("Напишите текст отзыва.");

        var existingReview = await db.BookReviews
            .FirstOrDefaultAsync(x =>
                x.BookId == bookId && x.UserId == userId);

        if (existingReview != null)
        {
            existingReview.Rating = rating;
            existingReview.Comment = comment.Trim();
            existingReview.CreatedAt = DateTime.UtcNow;
        }
        else
        {
            db.BookReviews.Add(new BookReview
            {
                BookId = bookId,
                UserId = userId,
                UserName = userName,
                Rating = rating,
                Comment = comment.Trim(),
                CreatedAt = DateTime.UtcNow
            });
        }

        await db.SaveChangesAsync();
    }

    // Подсчёт среднего рейтинга
    public async Task<decimal> GetAverageRatingAsync(int bookId)
    {
        var reviews = db.BookReviews
            .Where(x => x.BookId == bookId);

        if (!await reviews.AnyAsync())
            return 0;

        return await reviews.AverageAsync(x => (decimal)x.Rating);
    }
}