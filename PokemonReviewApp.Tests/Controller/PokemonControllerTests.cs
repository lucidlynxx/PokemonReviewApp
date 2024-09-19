using AutoMapper;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using PokemonReviewApp.Controllers;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Filter;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Tests.Controller;

public class PokemonControllerTests
{
    private readonly IPokemonRepository _pokemonRepository;
    private readonly IReviewRepository _reviewRepository;
    private readonly IMapper _mapper;
    private readonly IUriService _uriService;

    public PokemonControllerTests()
    {
        _pokemonRepository = A.Fake<IPokemonRepository>();
        _reviewRepository = A.Fake<IReviewRepository>();
        _mapper = A.Fake<IMapper>();
        _uriService = A.Fake<IUriService>();
    }

    [Fact]
    public void PokemonController_GetPokemons_ReturnOK()
    {
        //TODO Arrange
        var pokemons = A.Fake<ICollection<PokemonDto>>();
        var pokemonList = A.Fake<List<PokemonDto>>();

        A.CallTo(() => _mapper.Map<List<PokemonDto>>(pokemons)).Returns(pokemonList);

        var filter = new PaginationFilter
        {
            PageNumber = 1,
            PageSize = 10
        };

        var controller = new PokemonController(_pokemonRepository, _reviewRepository, _mapper, _uriService);

        //TODO Act
        var result = controller.GetPokemons(filter);

        //TODO Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<IActionResult>));
        result.IsCompleted.Should().BeTrue();
        controller.Ok().Should().BeOfType(typeof(OkResult), "because Http StatusCode 200");
    }

    [Fact]
    public void PokemonController_CreatePokemon_ReturnOK()
    {
        //TODO Arrange
        int ownerId = 1;
        int catId = 2;
        var pokemon = A.Fake<Pokemon>();
        var pokemonDto = A.Fake<PokemonDto>();
        var pokemons = A.Fake<ICollection<PokemonDto>>();
        var pokemonList = A.Fake<List<PokemonDto>>();
        A.CallTo(() => _pokemonRepository.GetPokemonTrimToUpper(pokemonDto)).Returns(pokemon);
        A.CallTo(() => _mapper.Map<Pokemon>(pokemonDto)).Returns(pokemon);
        A.CallTo(() => _pokemonRepository.CreatePokemon(ownerId, catId, pokemon)).Returns(true);
        var controller = new PokemonController(_pokemonRepository, _reviewRepository, _mapper, _uriService);

        //TODO Act
        var result = controller.CreatePokemon(ownerId, catId, pokemonDto);

        //TODO Assert
        result.Should().NotBeNull();
        result.Should().BeOfType(typeof(Task<IActionResult>));
        result.IsCompleted.Should().BeTrue();
    }
}
