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
public class CountryController : Controller
{
    private readonly ICountryRepository _countryRepository;
    private readonly IOwnerRepository _ownerRepository;
    private readonly IMapper _mapper;
    private readonly IUriService _uriService;

    public CountryController(ICountryRepository countryRepository, IOwnerRepository ownerRepository, IMapper mapper, IUriService uriService)
    {
        _countryRepository = countryRepository;
        _mapper = mapper;
        _uriService = uriService;
        _ownerRepository = ownerRepository;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
    public async Task<IActionResult> GetCountries([FromQuery] PaginationFilter filter)
    {
        var countries = _mapper.Map<List<CountryDto>>(await _countryRepository.GetCountries());

        var route = Request.Path.Value;

        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var pagedData = countries.Skip((filter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        if (pagedData == null || !pagedData.Any())
        {
            return StatusCode(404, "No countries found.");
        }

        var totalRecords = countries.Count();

        var pagedResponse = PaginationHelper.CreatedPagedResponse<CountryDto>(pagedData, validFilter, totalRecords, _uriService, route!);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pagedResponse);
    }

    [HttpGet("{countryId}")]
    [ProducesResponseType(200, Type = typeof(Country))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetCountry(int countryId)
    {
        if (!await _countryRepository.CountryExists(countryId))
            return NotFound();

        var country = _mapper.Map<CountryDto>(await _countryRepository.GetCountry(countryId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(new Response<CountryDto>(country));
    }

    [HttpGet("/api/Country/Owner/{ownerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(200, Type = typeof(Country))]
    public async Task<IActionResult> GetCountryOfAnOwner(int ownerId)
    {
        if (!await _ownerRepository.OwnerExists(ownerId))
            return NotFound();

        var country = _mapper.Map<CountryDto>(await _countryRepository.GetCountryByOwner(ownerId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(new Response<CountryDto>(country));
    }

    [HttpGet("{countryId}/Owners")]
    [ProducesResponseType(400)]
    [ProducesResponseType(200, Type = typeof(ICollection<Owner>))]
    public async Task<IActionResult> GetOwnersOfACountry(int countryId, [FromQuery] PaginationFilter filter)
    {
        if (!await _countryRepository.CountryExists(countryId))
            return NotFound();

        var owners = _mapper.Map<List<OwnerDto>>(await _countryRepository.GetOwnersFromACountry(countryId));

        var route = Request.Path.Value;

        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var pagedData = owners.Skip((filter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        if (pagedData == null || !pagedData.Any())
        {
            return StatusCode(404, "No owners found for this country.");
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
    public async Task<IActionResult> CreateCountry([FromBody] CountryDto countryDto)
    {
        if (countryDto == null)
            return BadRequest(ModelState);

        var country = await _countryRepository.GetCountryTrimToUpper(countryDto);

        if (country != null)
        {
            ModelState.AddModelError("", "Country already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var countryMap = _mapper.Map<Country>(countryDto);

        if (!await _countryRepository.CreateCountry(countryMap))
        {
            ModelState.AddModelError("", "Something went wrong while savin");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{countryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateCategory(int countryId, [FromBody] CountryDto countryDto)
    {
        if (countryDto == null)
            return BadRequest(ModelState);

        if (countryId != countryDto.Id)
            return BadRequest(ModelState);

        if (!await _countryRepository.CountryExists(countryId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var countryMap = _mapper.Map<Country>(countryDto);

        if (!await _countryRepository.UpdateCountry(countryMap))
        {
            ModelState.AddModelError("", "Something went wrong updating country");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{countryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteCountry(int countryId)
    {
        if (!await _countryRepository.CountryExists(countryId))
        {
            return NotFound();
        }

        var countryToDelete = await _countryRepository.GetCountry(countryId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _countryRepository.DeleteCountry(countryToDelete!))
        {
            ModelState.AddModelError("", "Something went wrong deleting country");
        }

        return NoContent();
    }
}
