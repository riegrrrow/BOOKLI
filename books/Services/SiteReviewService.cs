using books.Data;
using books.Models;
using Microsoft.EntityFrameworkCore;

namespace books.Services
{
    public class SiteReviewService
    {
        private readonly ApplicationDbContext db;

        public SiteReviewService(ApplicationDbContext context)
        {
            db = context;
        }

        public async Task<List<SiteReview>> GetReviewsAsync()
        {
            return await db.SiteReviews
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task AddReviewAsync(
            string userId,
            string userName,
            int rating,
            string comment)
        {
            if (rating < 1 || rating > 5)
                throw new ArgumentException("Оценка должна быть от 1 до 5.");

            if (string.IsNullOrWhiteSpace(comment))
                throw new ArgumentException("Напишите текст отзыва.");

            var review = await db.SiteReviews
                .FirstOrDefaultAsync(r => r.UserId == userId);

            if (review == null)
            {
                review = new SiteReview
                {
                    UserId = userId
                };

                db.SiteReviews.Add(review);
            }

            review.UserName = userName;
            review.Rating = rating;
            review.Comment = comment.Trim();
            review.CreatedAt = DateTime.UtcNow;

            await db.SaveChangesAsync();
        }

        public async Task<decimal> GetAverageRatingAsync()
        {
            if (!await db.SiteReviews.AnyAsync())
                return 0;

            return await db.SiteReviews
                .AverageAsync(r => (decimal)r.Rating);
        }
    }
}