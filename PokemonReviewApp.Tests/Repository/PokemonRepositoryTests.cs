using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Models;
using PokemonReviewApp.Repository;

namespace PokemonReviewApp.Tests.Repository;

public class PokemonRepositoryTests
{
    private async Task<DataContext> GetDatabaseContext()
    {
        var options = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        var databaseContext = new DataContext(options);
        databaseContext.Database.EnsureCreated();
        if (await databaseContext.Pokemon.CountAsync() <= 0)
        {
            for (int i = 0; i < 10; i++)
            {
                databaseContext.Pokemon.Add(
                    new Pokemon()
                    {
                        Name = "Pikachu",
                        BirthDate = new DateTime(1903, 1, 1),
                        PokemonCategories = new List<PokemonCategory>()
                            {
                                new PokemonCategory { Category = new Category() { Name = "Electric"}}
                            },
                        Reviews = new List<Review>()
                            {
                                new Review { Title="Pikachu",Text = "Pickahu is the best pokemon, because it is electric", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },
                                new Review { Title="Pikachu", Text = "Pickachu is the best a killing rocks", Rating = 5,
                                Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
                                new Review { Title="Pikachu",Text = "Pickchu, pickachu, pikachu", Rating = 1,
                                Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
                            }
                    }
                );
                await databaseContext.SaveChangesAsync();
            }
        }
        return databaseContext;
    }

    [Fact]
    public async void PokemonRepository_GetPokemon_ReturnsPokemon()
    {
        //TODO Arrange
        var name = "Pikachu";
        var dbContext = await GetDatabaseContext();
        var pokemonRepository = new PokemonRepository(dbContext);

        //TODO Act
        var result = await pokemonRepository.GetPokemon(name);

        //TODO Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<Pokemon>();
    }

    [Fact]
    public async void PokemonRepository_GetPokemonRating_ReturnDecimalBetweenOneAndTen()
    {
        //TODO Arrange
        var pokeId = 1;
        var dbContext = await GetDatabaseContext();
        var pokemonRepository = new PokemonRepository(dbContext);

        //TODO Act
        var result = await pokemonRepository.GetPokemonRating(pokeId);

        //TODO Assert
        result.Should().NotBe(0);

        int IntResult = Convert.ToInt32(result);

        IntResult.Should().BeInRange(1, 10);
    }
}
