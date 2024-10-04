using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApp.Dto;

public class LoginCredentialDto
{
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(100, ErrorMessage = "Username must be less than 100 characters.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
    public string? Password { get; set; }
}
