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
public class ReviewController : Controller
{
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IUriService _uriService;

    public ReviewController(IReviewRepository reviewRepository, IMapper mapper, IPokemonRepository pokemonRepository, IReviewerRepository reviewerRepository, IUriService uriService)
    {
        _reviewRepository = reviewRepository;
        _mapper = mapper;
        _pokemonRepository = pokemonRepository;
        _reviewerRepository = reviewerRepository;
        _uriService = uriService;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
    public async Task<IActionResult> GetReviews([FromQuery] PaginationFilter filter)
    {
        var reviews = _mapper.Map<List<ReviewDto>>(await _reviewRepository.GetReviews());

        var route = Request.Path.Value;

        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var pagedData = reviews.Skip((filter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        if (pagedData == null || !pagedData.Any())
        {
            return StatusCode(404, "No reviews found.");
        }

        var totalRecords = reviews.Count();

        var pagedResponse = PaginationHelper.CreatedPagedResponse<ReviewDto>(pagedData, validFilter, totalRecords, _uriService, route!);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pagedResponse);
    }

    [HttpGet("{reviewId}")]
    [ProducesResponseType(200, Type = typeof(Review))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetReview(int reviewId)
    {
        if (!await _reviewRepository.ReviewExists(reviewId))
            return NotFound();

        var review = _mapper.Map<ReviewDto>(await _reviewRepository.GetReview(reviewId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(new Response<ReviewDto>(review));
    }

    [HttpGet("Pokemon/{pokeId}")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetReviewsForAPokemon(int pokeId, [FromQuery] PaginationFilter filter)
    {
        if (!await _pokemonRepository.PokemonExists(pokeId))
            return NotFound();

        var reviews = _mapper.Map<List<ReviewDto>>(await _reviewRepository.GetReviewsOfAPokemon(pokeId));

        var route = Request.Path.Value;

        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var pagedData = reviews.Skip((filter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        if (pagedData == null || !pagedData.Any())
        {
            return StatusCode(404, "No reviews found for this pokemon.");
        }

        var totalRecords = reviews.Count();

        var pagedResponse = PaginationHelper.CreatedPagedResponse<ReviewDto>(pagedData, validFilter, totalRecords, _uriService, route!);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pagedResponse);
    }

    [HttpGet("/api/ReviewsWithReviewerAndPokemon")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<ReviewsWithReviewerAndPokemonDto>))]
    public async Task<IActionResult> GetReviewsWithReviewerAndPokemon([FromQuery] PaginationFilter filter)
    {
        var reviews = await _reviewRepository.GetReviewsWithReviewerAndPokemon();

        var route = Request.Path.Value;

        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var pagedData = reviews.Skip((filter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        if (pagedData == null || !pagedData.Any())
        {
            return StatusCode(404, "No reviews found.");
        }

        var totalRecords = reviews.Count();

        var pagedResponse = PaginationHelper.CreatedPagedResponse<ReviewsWithReviewerAndPokemonDto>(pagedData, validFilter, totalRecords, _uriService, route!);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pagedResponse);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateReview([FromQuery] int reviewerId, [FromQuery] int pokemonId, [FromBody] ReviewDto reviewDto)
    {
        if (reviewDto == null)
            return BadRequest(ModelState);

        var review = await _reviewRepository.GetReviewTrimToUpper(reviewDto);

        if (review != null)
        {
            ModelState.AddModelError("", "Review already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var reviewMap = _mapper.Map<Review>(reviewDto);

        reviewMap.Pokemon = await _pokemonRepository.GetPokemon(pokemonId);
        reviewMap.Reviewer = await _reviewerRepository.GetReviewer(reviewerId);

        if (!await _reviewRepository.CreateReview(reviewMap))
        {
            ModelState.AddModelError("", "Something went wrong while savin");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{reviewId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] ReviewDto reviewDto)
    {
        if (reviewDto == null)
            return BadRequest(ModelState);

        if (reviewId != reviewDto.Id)
            return BadRequest(ModelState);

        if (!await _reviewRepository.ReviewExists(reviewId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var reviewMap = _mapper.Map<Review>(reviewDto);

        if (!await _reviewRepository.UpdateReview(reviewMap))
        {
            ModelState.AddModelError("", "Something went wrong updating review");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{reviewId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteReview(int reviewId)
    {
        if (!await _reviewRepository.ReviewExists(reviewId))
        {
            return NotFound();
        }

        var reviewToDelete = await _reviewRepository.GetReview(reviewId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _reviewRepository.DeleteReview(reviewToDelete!))
        {
            ModelState.AddModelError("", "Something went wrong deleting review");
        }

        return NoContent();
    }
}
