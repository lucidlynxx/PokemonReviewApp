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
public class CategoryController : Controller
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;
    private readonly IUriService _uriService;

    public CategoryController(ICategoryRepository categoryRepository, IMapper mapper, IUriService uriService)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
        _uriService = uriService;
    }

    [Authorize(Policy = "UserOnly")]
    [HttpGet]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
    public async Task<IActionResult> GetCategories([FromQuery] PaginationFilter filter)
    {
        var categories = _mapper.Map<List<CategoryDto>>(await _categoryRepository.GetCategories());

        var route = Request.Path.Value;

        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var pagedData = categories.Skip((filter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        if (pagedData == null || !pagedData.Any())
        {
            return StatusCode(404, "No categories found.");
        }

        var totalRecords = categories.Count();

        var pagedResponse = PaginationHelper.CreatedPagedResponse<CategoryDto>(pagedData, validFilter, totalRecords, _uriService, route!);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(pagedResponse);
    }

    [Authorize(Policy = "UserOnly")]
    [HttpGet("{categoryId}")]
    [ProducesResponseType(200, Type = typeof(Category))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetCategory(int categoryId)
    {
        if (!await _categoryRepository.CategoryExists(categoryId))
            return NotFound();

        var category = _mapper.Map<CategoryDto>(await _categoryRepository.GetCategory(categoryId));

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        return Ok(new Response<CategoryDto>(category));
    }

    [Authorize(Policy = "UserOnly")]
    [HttpGet("{categoryId}/Pokemons")]
    [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
    [ProducesResponseType(400)]
    public async Task<IActionResult> GetPokemonsByCategoryId(int categoryId, [FromQuery] PaginationFilter filter)
    {
        if (!await _categoryRepository.CategoryExists(categoryId))
            return NotFound();

        var pokemons = _mapper.Map<List<PokemonDto>>(await _categoryRepository.GetPokemonsByCategory(categoryId));

        var route = Request.Path.Value;

        var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

        var pagedData = pokemons.Skip((filter.PageNumber - 1) * validFilter.PageSize)
            .Take(validFilter.PageSize)
            .ToList();

        if (pagedData == null || !pagedData.Any())
        {
            return StatusCode(404, "No pokemons found for this category.");
        }

        var totalRecords = pokemons.Count();

        var pagedResponse = PaginationHelper.CreatedPagedResponse<PokemonDto>(pagedData, validFilter, totalRecords, _uriService, route!);

        if (!ModelState.IsValid)
            return BadRequest();

        return Ok(pagedResponse);
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPost]
    [ProducesResponseType(204)]
    [ProducesResponseType(400)]
    public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
    {
        if (categoryDto == null)
            return BadRequest(ModelState);

        var category = await _categoryRepository.GetCategoryTrimToUpper(categoryDto);

        if (category != null)
        {
            ModelState.AddModelError("", "Category already exists.");
            return StatusCode(422, ModelState);
        }

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var categoryMap = _mapper.Map<Category>(categoryDto);

        if (!await _categoryRepository.CreateCategory(categoryMap))
        {
            ModelState.AddModelError("", "Something went wrong while savin");
            return StatusCode(500, ModelState);
        }

        return Ok("Successfully created");
    }

    [Authorize(Policy = "AdminOnly")]
    [HttpPut("{categoryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] CategoryDto categoryDto)
    {
        if (categoryDto == null)
            return BadRequest(ModelState);

        if (categoryId != categoryDto.Id)
            return BadRequest(ModelState);

        if (!await _categoryRepository.CategoryExists(categoryId))
            return NotFound();

        if (!ModelState.IsValid)
            return BadRequest();

        var categoryMap = _mapper.Map<Category>(categoryDto);

        if (!await _categoryRepository.UpdateCategory(categoryMap))
        {
            ModelState.AddModelError("", "Something went wrong updating category");
            return StatusCode(500, ModelState);
        }

        return NoContent();
    }

    [Authorize(Policy = "AdminOny")]
    [HttpDelete("{categoryId}")]
    [ProducesResponseType(400)]
    [ProducesResponseType(204)]
    [ProducesResponseType(404)]
    public async Task<IActionResult> DeleteCategory(int categoryId)
    {
        if (!await _categoryRepository.CategoryExists(categoryId))
            return NotFound();

        var categoryToDelete = await _categoryRepository.GetCategory(categoryId);

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        if (!await _categoryRepository.DeleteCategory(categoryToDelete!))
        {
            ModelState.AddModelError("", "Something went wrong deleting category");
        }

        return NoContent();
    }
}
