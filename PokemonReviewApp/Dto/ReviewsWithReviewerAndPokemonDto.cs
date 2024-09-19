using PokemonReviewApp.Models;

namespace PokemonReviewApp.Dto;

public class ReviewsWithReviewerAndPokemonDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Text { get; set; }
    public int Rating { get; set; }
    public ReviewerDto? Reviewer { get; set; }
    public PokemonDto? Pokemon { get; set; }
}
