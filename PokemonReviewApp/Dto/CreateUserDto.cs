using System.ComponentModel.DataAnnotations;
using PokemonReviewApp.Dto.Validation;

namespace PokemonReviewApp.Dto;

public class CreateUserDto
{
    [Required(ErrorMessage = "Username is required.")]
    [StringLength(100, ErrorMessage = "Username must be less than 100 characters.")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters.")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    public string? Email { get; set; }

    [Required(ErrorMessage = "Roles is required."), MinLength(1)]
    [ValidateRoles]
    public List<string> Roles { get; set; } = new List<string>();
}
