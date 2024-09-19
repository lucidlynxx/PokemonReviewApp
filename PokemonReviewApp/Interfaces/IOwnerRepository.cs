using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces;

public interface IOwnerRepository
{
    Task<ICollection<Owner>> GetOwners();
    Task<List<OwnersWithCountriesDto>> GetOwnersWithCountries();
    Task<List<OwnersWithPokemonsDto>> GetOwnersWithPokemons();
    Task<Owner?> GetOwner(int ownerId);
    Task<ICollection<Owner?>> GetOwnersOfAPokemon(int pokeId);
    Task<ICollection<Pokemon?>> GetPokemonsByOwner(int ownerId);
    Task<Owner> GetOwnerTrimToUpper(OwnerDto ownerDto);
    Task<bool> OwnerExists(int ownerId);
    Task<bool> CreateOwner(Owner owner);
    Task<bool> UpdateOwner(Owner owner);
    Task<bool> DeleteOwner(Owner owner);
    Task<bool> Save();
}
