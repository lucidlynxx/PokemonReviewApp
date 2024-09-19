using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public class PokemonRepository : IPokemonRepository
{
    private readonly DataContext _context;

    public PokemonRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<List<Pokemon>> GetPokemons()
    {
        return await _context.Pokemon.OrderBy(p => p.Id).ToListAsync();
    }

    public async Task<List<PokemonsWithCategoriesDto>> GetPokemonsWithCategories()
    {
        return await _context.Pokemon
            .GroupJoin(_context.PokemonCategories,
                pokemon => pokemon.Id,
                pokemonCategory => pokemonCategory.PokemonId,
                (pokemon, pokemonCategories) => new { pokemon, pokemonCategories })
            .SelectMany(
                pc => pc.pokemonCategories.DefaultIfEmpty(),
                (pc, pokemonCategory) => new { pc.pokemon, pokemonCategory })
            .GroupJoin(_context.Categories,
                pc => pc.pokemonCategory!.CategoryId,
                category => category.Id,
                (pc, categories) => new { pc.pokemon, pc.pokemonCategory, categories })
            .GroupBy(
                pc => pc.pokemon,
                pc => pc.categories.Select(category => new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                }).ToList())
            .Select(g => new PokemonsWithCategoriesDto
            {
                Id = g.Key.Id,
                Name = g.Key.Name,
                BirthDate = g.Key.BirthDate,
                Categories = g.SelectMany(categories => categories).ToList()
            })
            .ToListAsync();
    }

    public async Task<Pokemon?> GetPokemon(int id)
    {
        return await _context.Pokemon.Where(p => p.Id == id).FirstOrDefaultAsync()!;
    }

    public async Task<Pokemon?> GetPokemon(string name)
    {
        return await _context.Pokemon.Where(p => p.Name == name).FirstOrDefaultAsync()!;
    }

    public async Task<decimal> GetPokemonRating(int pokeId)
    {
        var review = await _context.Reviews.Where(p => p.Pokemon!.Id == pokeId).ToListAsync();

        if (review.Count() <= 0)
            return 0;

        return (decimal)review.Sum(r => r.Rating) / review.Count();
    }

    public async Task<bool> PokemonExists(int pokeId)
    {
        return await _context.Pokemon.AnyAsync(p => p.Id == pokeId);
    }

    public async Task<bool> CreatePokemon(int ownerId, int categoryId, Pokemon pokemon)
    {
        var pokemonOwnerEntity = await _context.Owners.Where(a => a.Id == ownerId).FirstOrDefaultAsync();
        var category = await _context.Categories.Where(a => a.Id == categoryId).FirstOrDefaultAsync();

        var pokemonOwner = new PokemonOwner()
        {
            Owner = pokemonOwnerEntity!,
            Pokemon = pokemon,
        };

        _context.Add(pokemonOwner);

        var pokemonCategory = new PokemonCategory()
        {
            Category = category!,
            Pokemon = pokemon,
        };

        _context.Add(pokemonCategory);

        _context.Add(pokemon);

        return await Save();
    }

    public async Task<bool> Save()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0 ? true : false;
    }

    public async Task<bool> UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon)
    {
        _context.Update(pokemon);
        return await Save();
    }

    public async Task<bool> DeletePokemon(Pokemon pokemon)
    {
        _context.Remove(pokemon);
        return await Save();
    }

    public async Task<Pokemon> GetPokemonTrimToUpper(PokemonDto pokemonDto)
    {
        var pokemons = await GetPokemons();

        return pokemons.Where(c => c.Name?.Trim().ToUpper() == pokemonDto.Name?.TrimEnd().ToUpper())
            .FirstOrDefault()!;
    }
}
