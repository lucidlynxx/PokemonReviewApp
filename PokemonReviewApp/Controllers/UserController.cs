using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Filter;
using PokemonReviewApp.Helper;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Wrappers;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class UserController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;
    private readonly IUriService _uriService;

    public UserController(IUserRepository userRepository, IMapper mapper, IUriService uriService)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _uriService = uriService;
    }

    [Authorize(Policy = "SuperAdminOnly")]
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<UserDto>))]
    public async Task<IActionResult> GetUsers([FromQuery] PaginationFilter filter)
    {
        string route = Request.Path.Value!;

        PaginationFilter validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        List<UserDto> users = _mapper.Map<List<UserDto>>(await _userRepository.GetUsersForPagedResponse(filter.PageNumber, validFilter.PageSize));

        if (users == null || !users.Any())
            return StatusCode(404, "No users found.");

        int totalRecords = users.Count();

        PagedResponse<List<UserDto>> pagedResponse = PaginationHelper.CreatedPagedResponse<UserDto>(users, validFilter, totalRecords, _uriService, route!);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pagedResponse);
    }

    [Authorize(Policy = "SuperAdminOnly")]
    [HttpGet("{userId}")]
    [ProducesResponseType(200, Type = typeof(UserDto))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetUser(int userId)
    {
        if (!await _userRepository.UserExistsById(userId))
            return NotFound();

        UserDto user = _mapper.Map<UserDto>(await _userRepository.GetUserById(userId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(new Response<UserDto>(user));
    }

    [Authorize(Policy = "SuperAdminOnly")]
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        if (createUserDto == null)
            return BadRequest(ModelState);

        User user = await _userRepository.GetUserTrimToUpper(createUserDto);

        if (user != null)
            return StatusCode(422, $"Username {user.Username} already exists.");

        if (await _userRepository.UserExistsByEmail(createUserDto.Email!))
            return StatusCode(422, $"Email {createUserDto.Email} is already taken.");

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        User userToCreate = new User()
        {
            Username = createUserDto.Username,
            Password = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password, workFactor: 4),
            Email = createUserDto.Email,
            Roles = createUserDto.Roles
        };

        if (!await _userRepository.CreateUser(userToCreate))
            return StatusCode(500, "Something went wrong while savin");

        return Ok("Successfully created");
    }

    [Authorize(Policy = "SuperAdminOnly")]
    [HttpPut("{userId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateUser(int userId, [FromBody] UserDto userDto)
    {
        if (userDto == null || userId != userDto.Id)
            return BadRequest(ModelState);

        if (!await _userRepository.UserExistsById(userId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        User? user = await _userRepository.GetUserByIdNoTracking(userId);

        User userToUpdate = new User()
        {
            Id = userDto.Id,
            Username = userDto.Username ?? user?.Username,
            Password = user?.Password,
            Email = userDto.Email ?? user?.Email,
            Roles = userDto.Roles ?? user?.Roles!,
        };

        if (!await _userRepository.UpdateUser(userToUpdate))
            return StatusCode(500, "Something went wrong while savin");

        return NoContent();
    }

    [Authorize(Policy = "SuperAdminOnly")]
    [HttpDelete("{userId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteUser(int userId)
    {
        if (!await _userRepository.UserExistsById(userId))
            return NotFound();

        User? userToDelete = await _userRepository.GetUserById(userId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _userRepository.DeleteUser(userToDelete!))
            return StatusCode(500, "Something went wrong while deleting user");

        return NoContent();
    }
}
