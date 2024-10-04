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
public class PokemonController : Controller
{
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;
    private readonly IUriService _uriService;

    public PokemonController(IPokemonRepository pokemonRepository, IReviewRepository reviewRepository, IMapper mapper, IUriService uriService)
    {
        _pokemonRepository = pokemonRepository;
        _reviewRepository = reviewRepository;
        _mapper = mapper;
        _uriService = uriService;
    }

    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    public async Task<IActionResult> GetPokemons([FromQuery] PaginationFilter filter)
    {
        var pokemons = _mapper.Map<List<PokemonDto>>(await _pokemonRepository.GetPokemons());

        var route = Request.Path.Value;

        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var pagedData = pokemons.Skip((filter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        if (pagedData == null || !pagedData.Any())
        {
            return StatusCode(404, "No pokemons found.");
        }

        var totalRecords = pokemons.Count();

        var pagedResponse = PaginationHelper.CreatedPagedResponse<PokemonDto>(pagedData, validFilter, totalRecords, _uriService, route!);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pagedResponse);
    }

    [Authorize(Policy = "UserOnly")]
    [HttpGet("/api/PokemonsWithCategories")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<PokemonsWithCategoriesDto>))]
    public async Task<IActionResult> GetPokemonsWithCategories([FromQuery] PaginationFilter filter)
    {
        var pokemons = await _pokemonRepository.GetPokemonsWithCategories();

        var route = Request.Path.Value;

        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var pagedData = pokemons.Skip((filter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        if (pagedData == null || !pagedData.Any())
        {
            return StatusCode(404, "No pokemons with categories found.");
        }

        var totalRecords = pokemons.Count();

        var pagedResponse = PaginationHelper.CreatedPagedResponse<PokemonsWithCategoriesDto>(pagedData, validFilter, totalRecords, _uriService, route!);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pagedResponse);
    }

    [Authorize(Policy = "UserOnly")]
    [HttpGet("{pokeId}")]
    [ProducesResponseType(200, Type = typeof(Pokemon))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetPokemon(int pokeId)
    {
        if (!await _pokemonRepository.PokemonExists(pokeId))
            return NotFound();

        var pokemon = _mapper.Map<PokemonDto>(await _pokemonRepository.GetPokemon(pokeId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(new Response<PokemonDto>(pokemon));
    }

    [Authorize(Policy = "UserOnly")]
    [HttpGet("{pokeId}/Rating")]
    [ProducesResponseType(200, Type = typeof(decimal))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetPokemonRating(int pokeId)
    {
        if (!await _pokemonRepository.PokemonExists(pokeId))
            return NotFound();

        var rating = await _pokemonRepository.GetPokemonRating(pokeId);

        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(new Response<decimal>(rating));
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto pokemonDto)
    {
        if (pokemonDto == null)
            return BadRequest(ModelState);

        var pokemon = await _pokemonRepository.GetPokemonTrimToUpper(pokemonDto);

        if (pokemon != null)
        {
            ModelState.AddModelError("", "Pokemon already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var pokemonMap = _mapper.Map<Pokemon>(pokemonDto);

        if (!await _pokemonRepository.CreatePokemon(ownerId, categoryId, pokemonMap))
        {
            ModelState.AddModelError("", "Something went wrong while savin");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{pokeId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdatePokemon(int pokeId, [FromQuery] int ownerId, [FromQuery] int categoryId, [FromBody] PokemonDto pokemonDto)
    {
        if (pokemonDto == null)
            return BadRequest(ModelState);

        if (pokeId != pokemonDto.Id)
            return BadRequest(ModelState);

        if (!await _pokemonRepository.PokemonExists(pokeId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var pokemonMap = _mapper.Map<Pokemon>(pokemonDto);

        if (!await _pokemonRepository.UpdatePokemon(ownerId, categoryId, pokemonMap))
        {
            ModelState.AddModelError("", "Something went wrong updating pokemon");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpDelete("{pokemonId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeletePokemon(int pokemonId)
    {
        if (!await _pokemonRepository.PokemonExists(pokemonId))
        {
            return NotFound();
        }

        var reviewsToDelete = await _reviewRepository.GetReviewsOfAPokemon(pokemonId);
        var pokemonToDelete = await _pokemonRepository.GetPokemon(pokemonId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _reviewRepository.DeleteReviews(reviewsToDelete.ToList()))
        {
            ModelState.AddModelError("", "Something went wrong deleting reviews");
        }

        if (!await _pokemonRepository.DeletePokemon(pokemonToDelete!))
        {
            ModelState.AddModelError("", "Something went wrong deleting pokemon");
        }

        return NoContent();
    }
}
