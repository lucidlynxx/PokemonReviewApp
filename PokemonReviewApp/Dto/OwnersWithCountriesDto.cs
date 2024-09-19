using PokemonReviewApp.Models;

namespace PokemonReviewApp.Dto;

public class OwnersWithCountriesDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Gym { get; set; }
    public CountryDto? Country { get; set; }
}
