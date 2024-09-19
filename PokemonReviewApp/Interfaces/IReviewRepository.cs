using PokemonReviewApp.Models;
using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Interfaces;

public interface IReviewRepository
{
    Task<ICollection<Review>> GetReviews();
    Task<Review?> GetReview(int reviewId);
    Task<List<ReviewsWithReviewerAndPokemonDto>> GetReviewsWithReviewerAndPokemon();
    Task<ICollection<Review>> GetReviewsOfAPokemon(int pokeId);
    Task<Review> GetReviewTrimToUpper(ReviewDto reviewDto);
    Task<bool> ReviewExists(int reviewId);
    Task<bool> CreateReview(Review review);
    Task<bool> UpdateReview(Review review);
    Task<bool> DeleteReview(Review review);
    Task<bool> DeleteReviews(List<Review> reviews);
    Task<bool> Save();
}
