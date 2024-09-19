using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces;

public interface ICategoryRepository
{
    Task<ICollection<Category>> GetCategories();
    Task<Category?> GetCategory(int id);
    Task<ICollection<Pokemon?>> GetPokemonsByCategory(int categoryId);
    Task<Category> GetCategoryTrimToUpper(CategoryDto categoryDto);
    Task<bool> CategoryExists(int id);
    Task<bool> CreateCategory(Category category);
    Task<bool> UpdateCategory(Category category);
    Task<bool> DeleteCategory(Category category);
    Task<bool> Save();
}
