using System;

namespace PokemonReviewApp.Dto;

public class UserDto
{
    public int Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
}
