using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public class CategoryRepository : ICategoryRepository
{
    private readonly DataContext _context;

    public CategoryRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ICollection<Category>> GetCategories()
    {
        return await _context.Categories.ToListAsync();
    }

    public async Task<Category?> GetCategory(int id)
    {
        return await _context.Categories.Where(c => c.Id == id).FirstOrDefaultAsync()!;
    }

    public async Task<ICollection<Pokemon?>> GetPokemonsByCategory(int categoryId)
    {
        return await _context.PokemonCategories.Where(pc => pc.CategoryId == categoryId).Select(p => p.Pokemon).ToListAsync()!;
    }

    public async Task<bool> CategoryExists(int id)
    {
        return await _context.Categories.AnyAsync(c => c.Id == id);
    }

    public async Task<bool> CreateCategory(Category category)
    {
        //* Change Tracker
        //* add, updating, modifying,
        //* connected vs disconnected
        //* EntityState.Added
        _context.Add(category);
        return await Save();
    }

    public async Task<bool> Save()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0 ? true : false;
    }

    public async Task<bool> UpdateCategory(Category category)
    {
        _context.Update(category);
        return await Save();
    }

    public async Task<bool> DeleteCategory(Category category)
    {
        _context.Remove(category);
        return await Save();
    }

    public async Task<Category> GetCategoryTrimToUpper(CategoryDto categoryDto)
    {
        var categories = await GetCategories();

        return categories.Where(c => c.Name?.Trim().ToUpper() == categoryDto.Name?.Trim().ToUpper())
            .FirstOrDefault()!;
    }
}
