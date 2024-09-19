using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Repository;

public class ReviewRepository : IReviewRepository
{
    private readonly DataContext _context;

    public ReviewRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateReview(Review review)
    {
        _context.Add(review);
        return await Save();
    }

    public async Task<bool> DeleteReview(Review review)
    {
        _context.Remove(review);
        return await Save();
    }

    public async Task<bool> DeleteReviews(List<Review> reviews)
    {
        _context.RemoveRange(reviews);
        return await Save();
    }

    public async Task<Review?> GetReview(int reviewId)
    {
        return await _context.Reviews.Where(r => r.Id == reviewId).FirstOrDefaultAsync()!;
    }

    public async Task<ICollection<Review>> GetReviews()
    {
        return await _context.Reviews.ToListAsync();
    }

    public async Task<ICollection<Review>> GetReviewsOfAPokemon(int pokeId)
    {
        return await _context.Reviews.Where(r => r.Pokemon!.Id == pokeId).ToListAsync();
    }

    public async Task<bool> ReviewExists(int reviewId)
    {
        return await _context.Reviews.AnyAsync(r => r.Id == reviewId);
    }

    public async Task<bool> Save()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0 ? true : false;
    }

    public async Task<bool> UpdateReview(Review review)
    {
        _context.Update(review);
        return await Save();
    }

    public async Task<Review> GetReviewTrimToUpper(ReviewDto reviewDto)
    {
        var reviews = await GetReviews();

        return reviews.Where(c => c.Title?.Trim().ToUpper() == reviewDto?.Title?.TrimEnd().ToUpper())
            .FirstOrDefault()!;
    }

    public async Task<List<ReviewsWithReviewerAndPokemonDto>> GetReviewsWithReviewerAndPokemon()
    {
        return await _context.Reviews
            .Join(_context.Reviewers,
                review => review.Reviewer!.Id,
                reviewer => reviewer.Id,
                (review, reviewer) => new { review, reviewer })
            .Join(_context.Pokemon,
                rr => rr.review.Pokemon!.Id,
                pokemon => pokemon.Id,
                (rr, pokemon) => new ReviewsWithReviewerAndPokemonDto
                {
                    Id = rr.review.Id,
                    Title = rr.review.Title,
                    Text = rr.review.Text,
                    Rating = rr.review.Rating,
                    Reviewer = new ReviewerDto
                    {
                        Id = rr.review.Reviewer!.Id,
                        FirstName = rr.review.Reviewer!.FirstName,
                        LastName = rr.review.Reviewer!.LastName
                    },
                    Pokemon = new PokemonDto
                    {
                        Id = pokemon.Id,
                        Name = pokemon.Name,
                        BirthDate = pokemon.BirthDate,
                    }
                })
            .ToListAsync();
    }
}
