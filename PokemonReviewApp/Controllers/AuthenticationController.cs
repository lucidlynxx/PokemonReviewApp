using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Models.Enum;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _userRepository;

    public AuthenticationController(IConfiguration configuration, IUserRepository userRepository)
    {
        _configuration = configuration;
        _userRepository = userRepository;
    }

    [HttpPost("Login")]
    [Produces("application/json")]
    public async Task<IActionResult> Login([FromBody] LoginCredentialDto request)
    {
        if (request == null)
            return BadRequest(ModelState);

        var user = await _userRepository.GetUserByUsername(request.Username!);

        if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
            return Unauthorized("Invalid user credentials.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var token = IssueToken(user);

        return Ok(new { Token = token });
    }

    [HttpPost("Register")]
    [Produces("application/json")]
    public async Task<IActionResult> Register([FromBody] RegisterDto request)
    {
        if (request == null)
            return BadRequest(ModelState);

        if (await _userRepository.UserExistsByUsername(request.Username!))
            return StatusCode(422, $"Username {request.Username} is already taken.");

        if (await _userRepository.UserExistsByEmail(request.Email!))
            return StatusCode(422, $"Email {request.Email} is already taken.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var user = new User()
        {
            Username = request.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(request.Password, workFactor: 4),
            Email = request.Email,
            Roles = new List<string> { Role.user.ToString() },
        };

        if (!await _userRepository.CreateUser(user))
            return StatusCode(500, "Something went wrong while savin");

        return Ok("Successfully Created");
    }

    private string IssueToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!));

        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, user.Email!),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.Username!),
        };

        user.Roles.ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_configuration.GetValue<int>("Jwt:ExpirationTimeInMinutes")),
            SigningCredentials = credentials,
            Issuer = _configuration["Jwt:Issuer"],
            Audience = _configuration["Jwt:Audience"],
        };

        var handler = new JsonWebTokenHandler();

        string token = handler.CreateToken(tokenDescriptor);

        return token;
    }
}
