using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Wrappers;
using PokemonReviewApp.Filter;
using PokemonReviewApp.Helper;

namespace PokemonReviewApp.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OwnerController : Controller
{
    private readonly IOwnerRepository _ownerRepository;
    private readonly ICountryRepository _countryRepository;
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IMapper _mapper;
    private readonly IUriService _uriService;

    public OwnerController(IOwnerRepository ownerRepository, ICountryRepository countryRepository, IPokemonRepository pokemonRepository, IMapper mapper, IUriService uriService)
    {
        _ownerRepository = ownerRepository;
        _countryRepository = countryRepository;
        _pokemonRepository = pokemonRepository;
        _mapper = mapper;
        _uriService = uriService;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
    public async Task<IActionResult> GetOwners([FromQuery] PaginationFilter filter)
    {
        var owners = _mapper.Map<List<OwnerDto>>(await _ownerRepository.GetOwners());

        var route = Request.Path.Value;

        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var pagedData = owners.Skip((filter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        if (pagedData == null || !pagedData.Any())
        {
            return StatusCode(404, "No owners found.");
        }

        var totalRecords = owners.Count();

        var pagedResponse = PaginationHelper.CreatedPagedResponse<OwnerDto>(pagedData, validFilter, totalRecords, _uriService, route!);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pagedResponse);
    }

    [HttpGet("/api/OwnersWithCountries")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<OwnersWithCountriesDto>))]
    public async Task<IActionResult> GetOwnersWithCountries([FromQuery] PaginationFilter filter)
    {
        var owners = await _ownerRepository.GetOwnersWithCountries();

        var route = Request.Path.Value;

        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var pagedData = owners.Skip((filter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        if (pagedData == null || !pagedData.Any())
        {
            return StatusCode(404, "No owners with countries found.");
        }

        var totalRecords = owners.Count();

        var pagedResponse = PaginationHelper.CreatedPagedResponse<OwnersWithCountriesDto>(pagedData, validFilter, totalRecords, _uriService, route!);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pagedResponse);
    }

    [HttpGet("/api/OwnersWithPokemons")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<OwnersWithPokemonsDto>))]
    public async Task<IActionResult> GetOwnersWithPokemons([FromQuery] PaginationFilter filter)
    {
        var owners = await _ownerRepository.GetOwnersWithPokemons();

        var route = Request.Path.Value;

        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var pagedData = owners.Skip((filter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        if (pagedData == null || !pagedData.Any())
        {
            return StatusCode(404, "No owners with countries found.");
        }

        var totalRecords = owners.Count();

        var pagedResponse = PaginationHelper.CreatedPagedResponse<OwnersWithPokemonsDto>(pagedData, validFilter, totalRecords, _uriService, route!);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pagedResponse);
    }

    [HttpGet("{ownerId}")]
    [ProducesResponseType(200, Type = typeof(Owner))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetOwner(int ownerId)
    {
        if (!await _ownerRepository.OwnerExists(ownerId))
            return NotFound();

        var owner = _mapper.Map<OwnerDto>(await _ownerRepository.GetOwner(ownerId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(new Response<OwnerDto>(owner));
    }

    [HttpGet("{ownerId}/Pokemons")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetPokemonsByOwner(int ownerId, [FromQuery] PaginationFilter filter)
    {
        if (!await _ownerRepository.OwnerExists(ownerId))
            return NotFound();

        var pokemons = _mapper.Map<List<PokemonDto>>(await _ownerRepository.GetPokemonsByOwner(ownerId));

        var route = Request.Path.Value;

        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var pagedData = pokemons.Skip((filter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        if (pagedData == null || !pagedData.Any())
        {
            return StatusCode(404, "No pokemons found for this owner.");
        }

        var totalRecords = pokemons.Count();

        var pagedResponse = PaginationHelper.CreatedPagedResponse<PokemonDto>(pagedData, validFilter, totalRecords, _uriService, route!);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pagedResponse);
    }

    [HttpGet("/api/Owners/Pokemon/{pokeId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetOwnersByPokemon(int pokeId, [FromQuery] PaginationFilter filter)
    {
        if (!await _pokemonRepository.PokemonExists(pokeId))
            return NotFound();

        var owners = _mapper.Map<List<OwnerDto>>(await _ownerRepository.GetOwnersOfAPokemon(pokeId));

        var route = Request.Path.Value;

        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var pagedData = owners.Skip((filter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        if (pagedData == null || !pagedData.Any())
        {
            return StatusCode(404, "No owners found for this pokemon.");
        }

        var totalRecords = owners.Count();

        var pagedResponse = PaginationHelper.CreatedPagedResponse<OwnerDto>(pagedData, validFilter, totalRecords, _uriService, route!);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pagedResponse);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateOwner([FromQuery] int countryId, [FromBody] OwnerDto ownerDto)
    {
        if (ownerDto == null)
            return BadRequest(ModelState);

        var owner = await _ownerRepository.GetOwnerTrimToUpper(ownerDto);

        if (owner != null)
        {
            ModelState.AddModelError("", "Owner already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var ownerMap = _mapper.Map<Owner>(ownerDto);

        ownerMap.Country = await _countryRepository.GetCountry(countryId);

        if (!await _ownerRepository.CreateOwner(ownerMap))
        {
            ModelState.AddModelError("", "Something went wrong while savin");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{ownerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateOwner(int ownerId, [FromBody] OwnerDto ownerDto)
    {
        if (ownerDto == null)
            return BadRequest(ModelState);

        if (ownerId != ownerDto.Id)
            return BadRequest(ModelState);

        if (!await _ownerRepository.OwnerExists(ownerId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var ownerMap = _mapper.Map<Owner>(ownerDto);

        if (!await _ownerRepository.UpdateOwner(ownerMap))
        {
            ModelState.AddModelError("", "Something went wrong updating owner");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{ownerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteOwner(int ownerId)
    {
        if (!await _ownerRepository.OwnerExists(ownerId))
        {
            return NotFound();
        }

        var ownerToDelete = await _ownerRepository.GetOwner(ownerId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _ownerRepository.DeleteOwner(ownerToDelete!))
        {
            ModelState.AddModelError("", "Something went wrong deleting owner");
        }

        return NoContent();
    }
}
