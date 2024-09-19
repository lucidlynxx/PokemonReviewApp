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
public class ReviewerController : Controller
{
    private readonly IReviewerRepository _reviewerRepository;
    private readonly IMapper _mapper;
    private readonly IUriService _uriService;

    public ReviewerController(IReviewerRepository reviewerRepository, IMapper mapper, IUriService uriService)
    {
        _reviewerRepository = reviewerRepository;
        _mapper = mapper;
        _uriService = uriService;
    }

    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
    public async Task<IActionResult> GetReviewers([FromQuery] PaginationFilter filter)
    {
        var reviewers = _mapper.Map<List<ReviewerDto>>(await _reviewerRepository.GetReviewers());

        var route = Request.Path.Value;

        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var pagedData = reviewers.Skip((filter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        if (pagedData == null || !pagedData.Any())
        {
            return StatusCode(404, "No reviewers found.");
        }

        var totalRecords = reviewers.Count();

        var pagedResponse = PaginationHelper.CreatedPagedResponse<ReviewerDto>(pagedData, validFilter, totalRecords, _uriService, route!);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pagedResponse);
    }

    [HttpGet("{reviewerId}")]
    [ProducesResponseType(200, Type = typeof(Reviewer))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetReviewer(int reviewerId)
    {
        if (!await _reviewerRepository.ReviewerExists(reviewerId))
            return NotFound();

        var reviewer = _mapper.Map<ReviewerDto>(await _reviewerRepository.GetReviewer(reviewerId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(new Response<ReviewerDto>(reviewer));
    }

    [HttpGet("{reviewerId}/Reviews")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetReviewsByAReviewer(int reviewerId, [FromQuery] PaginationFilter filter)
    {
        if (!await _reviewerRepository.ReviewerExists(reviewerId))
            return NotFound();

        var reviews = _mapper.Map<List<ReviewDto>>(await _reviewerRepository.GetReviewsByReviewer(reviewerId));

        var route = Request.Path.Value;

        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var pagedData = reviews.Skip((filter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        if (pagedData == null || !pagedData.Any())
        {
            return StatusCode(404, "No reviews found for this reviewer.");
        }

        var totalRecords = reviews.Count();

        var pagedResponse = PaginationHelper.CreatedPagedResponse<ReviewDto>(pagedData, validFilter, totalRecords, _uriService, route!);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pagedResponse);
    }

    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateReviewer([FromBody] ReviewerDto reviewerDto)
    {
        if (reviewerDto == null)
            return BadRequest(ModelState);

        var reviewer = await _reviewerRepository.GetReviewerTrimToUpper(reviewerDto);

        if (reviewer != null)
        {
            ModelState.AddModelError("", "Reviewer already exists");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var reviewerMap = _mapper.Map<Reviewer>(reviewerDto);

        if (!await _reviewerRepository.CreateReviewer(reviewerMap))
        {
            ModelState.AddModelError("", "Something went wrong while savin");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [HttpPut("{reviewerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateReviewer(int reviewerId, [FromBody] ReviewerDto reviewerDto)
    {
        if (reviewerDto == null)
            return BadRequest(ModelState);

        if (reviewerId != reviewerDto.Id)
            return BadRequest(ModelState);

        if (!await _reviewerRepository.ReviewerExists(reviewerId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var reviewerMap = _mapper.Map<Reviewer>(reviewerDto);

        if (!await _reviewerRepository.UpdateReviewer(reviewerMap))
        {
            ModelState.AddModelError("", "Something went wrong updating reviewer");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [HttpDelete("{reviewerId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteReviewer(int reviewerId)
    {
        if (!await _reviewerRepository.ReviewerExists(reviewerId))
        {
            return NotFound();
        }

        var reviewerToDelete = await _reviewerRepository.GetReviewer(reviewerId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _reviewerRepository.DeleteReviewer(reviewerToDelete!))
        {
            ModelState.AddModelError("", "Something went wrong deleting reviewer");
        }

        return NoContent();
    }
}
