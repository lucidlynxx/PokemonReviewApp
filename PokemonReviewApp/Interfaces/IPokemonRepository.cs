using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces;

public interface IPokemonRepository
{
    Task<List<Pokemon>> GetPokemons();
    Task<List<PokemonsWithCategoriesDto>> GetPokemonsWithCategories();
    Task<Pokemon?> GetPokemon(int id);
    Task<Pokemon?> GetPokemon(string name);
    Task<Pokemon> GetPokemonTrimToUpper(PokemonDto pokemonDto);
    Task<decimal> GetPokemonRating(int pokeId);
    Task<bool> PokemonExists(int pokeId);
    Task<bool> CreatePokemon(int ownerId, int categoryId, Pokemon pokemon);
    Task<bool> UpdatePokemon(int ownerId, int categoryId, Pokemon pokemon);
    Task<bool> DeletePokemon(Pokemon pokemon);
    Task<bool> Save();
}
