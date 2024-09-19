using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public class OwnerRepository : IOwnerRepository
{
    private readonly DataContext _context;

    public OwnerRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateOwner(Owner owner)
    {
        _context.Add(owner);
        return await Save();
    }

    public async Task<bool> DeleteOwner(Owner owner)
    {
        _context.Remove(owner);
        return await Save();
    }

    public async Task<Owner?> GetOwner(int ownerId)
    {
        return await _context.Owners.Where(o => o.Id == ownerId).FirstOrDefaultAsync()!;
    }

    public async Task<ICollection<Owner?>> GetOwnersOfAPokemon(int pokeId)
    {
        return await _context.PokemonOwners.Where(p => p.Pokemon!.Id == pokeId).Select(o => o.Owner).ToListAsync()!;
    }

    public async Task<ICollection<Owner>> GetOwners()
    {
        return await _context.Owners.ToListAsync();
    }

    public async Task<List<OwnersWithCountriesDto>> GetOwnersWithCountries()
    {
        return await _context.Owners
            .Join(_context.Countries,
                owner => owner.Country!.Id,
                country => country.Id,
                (owner, country) => new OwnersWithCountriesDto
                {
                    Id = owner.Id,
                    FirstName = owner.FirstName,
                    LastName = owner.LastName,
                    Gym = owner.Gym,
                    Country = new CountryDto
                    {
                        Id = country.Id,
                        Name = country.Name,
                    }
                })
            .ToListAsync();
    }

    public async Task<List<OwnersWithPokemonsDto>> GetOwnersWithPokemons()
    {
        return await _context.Owners
            .GroupJoin(_context.PokemonOwners,
                owner => owner.Id,
                pokemonOwner => pokemonOwner.OwnerId,
                (owner, pokemonOwners) => new { owner, pokemonOwners })
            .SelectMany(
                po => po.pokemonOwners.DefaultIfEmpty(),
                (po, pokemonOwner) => new { po.owner, pokemonOwner })
            .GroupJoin(_context.Pokemon,
                po => po.pokemonOwner!.PokemonId,
                pokemon => pokemon.Id,
                (po, pokemons) => new { po.owner, po.pokemonOwner, pokemons })
            .GroupBy(
                po => po.owner,  // Group by owner
                po => po.pokemons.Select(pokemon => new PokemonDto
                {
                    Id = pokemon.Id,
                    Name = pokemon.Name,
                    BirthDate = pokemon.BirthDate
                }).ToList())  // Collect all Pokémon into a list
            .Select(g => new OwnersWithPokemonsDto
            {
                Id = g.Key.Id,
                FirstName = g.Key.FirstName,
                LastName = g.Key.LastName,
                Gym = g.Key.Gym,
                Pokemons = g.SelectMany(pokemons => pokemons).ToList()  // Combine all Pokémon into a single list
            })
            .ToListAsync();
    }

    public async Task<ICollection<Pokemon?>> GetPokemonsByOwner(int ownerId)
    {
        return await _context.PokemonOwners.Where(p => p.Owner!.Id == ownerId).Select(p => p.Pokemon).ToListAsync()!;
    }

    public async Task<bool> OwnerExists(int ownerId)
    {
        return await _context.Owners.AnyAsync(o => o.Id == ownerId);
    }

    public async Task<bool> Save()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0 ? true : false;
    }

    public async Task<bool> UpdateOwner(Owner owner)
    {
        _context.Update(owner);
        return await Save();
    }

    public async Task<Owner> GetOwnerTrimToUpper(OwnerDto ownerDto)
    {
        var owners = await GetOwners();

        return owners.Where(c => c.LastName?.Trim().ToUpper() == ownerDto.LastName?.TrimEnd().ToUpper())
            .FirstOrDefault()!;
    }
}
