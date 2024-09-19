using PokemonReviewApp.Data;
using PokemonReviewApp.Models;

namespace PokemonReviewApp;

public class Seed
{
    private readonly DataContext dataContext;
    public Seed(DataContext context)
    {
        dataContext = context;
    }
    public void SeedDataContext()
    {
        if (!dataContext.PokemonOwners.Any())
        {
            // var pokemonOwners = new List<PokemonOwner>()
            //     {
            //         new PokemonOwner()
            //         {
            //             Pokemon = new Pokemon()
            //             {
            //                 Name = "Pikachu",
            //                 BirthDate = new DateTime(1903,1,1),
            //                 PokemonCategories = new List<PokemonCategory>()
            //                 {
            //                     new PokemonCategory { Category = new Category() { Name = "Electric"}}
            //                 },
            //                 Reviews = new List<Review>()
            //                 {
            //                     new Review { Title="Pikachu",Text = "Pickahu is the best pokemon, because it is electric", Rating = 5,
            //                     Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },
            //                     new Review { Title="Pikachu", Text = "Pickachu is the best a killing rocks", Rating = 5,
            //                     Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
            //                     new Review { Title="Pikachu",Text = "Pickchu, pickachu, pikachu", Rating = 1,
            //                     Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
            //                 }
            //             },
            //             Owner = new Owner()
            //             {
            //                 FirstName = "Jack",
            //                 LastName = "London",
            //                 Gym = "Brocks Gym",
            //                 Country = new Country()
            //                 {
            //                     Name = "Kanto"
            //                 }
            //             }
            //         },
            //         new PokemonOwner()
            //         {
            //             Pokemon = new Pokemon()
            //             {
            //                 Name = "Squirtle",
            //                 BirthDate = new DateTime(1903,1,1),
            //                 PokemonCategories = new List<PokemonCategory>()
            //                 {
            //                     new PokemonCategory { Category = new Category() { Name = "Water"}}
            //                 },
            //                 Reviews = new List<Review>()
            //                 {
            //                     new Review { Title= "Squirtle", Text = "squirtle is the best pokemon, because it is electric", Rating = 5,
            //                     Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },
            //                     new Review { Title= "Squirtle",Text = "Squirtle is the best a killing rocks", Rating = 5,
            //                     Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
            //                     new Review { Title= "Squirtle", Text = "squirtle, squirtle, squirtle", Rating = 1,
            //                     Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
            //                 }
            //             },
            //             Owner = new Owner()
            //             {
            //                 FirstName = "Harry",
            //                 LastName = "Potter",
            //                 Gym = "Mistys Gym",
            //                 Country = new Country()
            //                 {
            //                     Name = "Saffron City"
            //                 }
            //             }
            //         },
            //         new PokemonOwner()
            //         {
            //             Pokemon = new Pokemon()
            //             {
            //                 Name = "Venasuar",
            //                 BirthDate = new DateTime(1903,1,1),
            //                 PokemonCategories = new List<PokemonCategory>()
            //                 {
            //                     new PokemonCategory { Category = new Category() { Name = "Leaf"}}
            //                 },
            //                 Reviews = new List<Review>()
            //                 {
            //                     new Review { Title="Veasaur",Text = "Venasuar is the best pokemon, because it is electric", Rating = 5,
            //                     Reviewer = new Reviewer(){ FirstName = "Teddy", LastName = "Smith" } },
            //                     new Review { Title="Veasaur",Text = "Venasuar is the best a killing rocks", Rating = 5,
            //                     Reviewer = new Reviewer(){ FirstName = "Taylor", LastName = "Jones" } },
            //                     new Review { Title="Veasaur",Text = "Venasuar, Venasuar, Venasuar", Rating = 1,
            //                     Reviewer = new Reviewer(){ FirstName = "Jessica", LastName = "McGregor" } },
            //                 }
            //             },
            //             Owner = new Owner()
            //             {
            //                 FirstName = "Ash",
            //                 LastName = "Ketchum",
            //                 Gym = "Ashs Gym",
            //                 Country = new Country()
            //                 {
            //                     Name = "Millet Town"
            //                 }
            //             }
            //         }
            //     };

            var pokemons = new List<Pokemon>()
                {
                    new Pokemon()
                    {
                        Name = "Pikachu",
                        BirthDate = new DateTime(1999, 1, 1),
                        Reviews = new List<Review>()
                        {
                            new Review()
                            {
                                Title = "Good Review",
                                Text = "This Pokemon is Awesome",
                                Rating = 5,
                                Reviewer = new Reviewer()
                                {
                                    FirstName = "Teddy",
                                    LastName = "Smith"
                                }
                            },
                            new Review()
                            {
                                Title = "Bad Review",
                                Text = "This Pokemon is Bad",
                                Rating = 3,
                                Reviewer = new Reviewer()
                                {
                                    FirstName = "Dzaky",
                                    LastName = "Syahrizal"
                                }
                            },new Review()
                            {
                                Title = "Soso Review",
                                Text = "This Pokemon is not bad",
                                Rating = 4,
                                Reviewer = new Reviewer()
                                {
                                    FirstName = "Benjamin",
                                    LastName = "Johnson"
                                }
                            },
                        }
                    },
                    new Pokemon()
                    {
                        Name = "Squirtle",
                        BirthDate = new DateTime(1999, 2, 2),
                        Reviews = new List<Review>()
                        {
                            new Review()
                            {
                                Title = "Good Review",
                                Text = "This Pokemon is Awesome",
                                Rating = 5,
                                Reviewer = new Reviewer()
                                {
                                    FirstName = "Jessica",
                                    LastName = "McGregor"
                                }
                            },
                            new Review()
                            {
                                Title = "Bad Review",
                                Text = "This Pokemon is Bad",
                                Rating = 3,
                                Reviewer = new Reviewer()
                                {
                                    FirstName = "Taylor",
                                    LastName = "Jones"
                                }
                            },new Review()
                            {
                                Title = "Soso Review",
                                Text = "This Pokemon is not bad",
                                Rating = 4,
                                Reviewer = new Reviewer()
                                {
                                    FirstName = "Kirk",
                                    LastName = "Lang"
                                }
                            },
                        }
                    },
                    new Pokemon()
                    {
                        Name = "Venasuar",
                        BirthDate = new DateTime(1999, 3, 3),
                        Reviews = new List<Review>()
                        {
                            new Review()
                            {
                                Title = "Good Review",
                                Text = "This Pokemon is Awesome",
                                Rating = 5,
                                Reviewer = new Reviewer()
                                {
                                    FirstName = "Lindsay",
                                    LastName = "Lohan"
                                }
                            },
                            new Review()
                            {
                                Title = "Bad Review",
                                Text = "This Pokemon is Bad",
                                Rating = 3,
                                Reviewer = new Reviewer()
                                {
                                    FirstName = "Ashley",
                                    LastName = "Borg"
                                }
                            },new Review()
                            {
                                Title = "Soso Review",
                                Text = "This Pokemon is not bad",
                                Rating = 4,
                                Reviewer = new Reviewer()
                                {
                                    FirstName = "Zoe",
                                    LastName = "Ziggler"
                                }
                            },
                        }
                    },
                    new Pokemon()
                    {
                        Name = "Bulbasaur",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Charmander",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Jigglypuff",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Gengar",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Eevee",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Snorlax",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Mewtwo",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Psyduck",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Machamp",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Gyarados",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Lapras",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Ditto",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Kabuto",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Dragonite",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Magikarp",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Vaporeon",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Jolteon",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Flareon",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Omastar",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Scyther",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Pinsir",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Tauros",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Aerodactyl",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Articuno",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Zapdos",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Moltres",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Dratini",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Mew",
                        BirthDate = new DateTime(1996, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Chikorita",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Cyndaquil",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Totodile",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Togepi",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Ampharos",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Espeon",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Umbreon",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Steelix",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Scizor",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Heracross",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Houndoom",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Kingdra",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Donphan",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Porygon2",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Blissey",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Raikou",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Entei",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Suicune",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                    new Pokemon()
                    {
                        Name = "Tyranitar",
                        BirthDate = new DateTime(1999, 2, 27),
                    },
                };

            var categories = new List<Category>()
                {
                    new Category()
                    {
                        Name = "Normal"
                    },
                    new Category()
                    {
                        Name = "Fire"
                    },
                    new Category()
                    {
                        Name = "Water"
                    },
                    new Category()
                    {
                        Name = "Electric"
                    },
                    new Category()
                    {
                        Name = "Grass"
                    },
                    new Category()
                    {
                        Name = "Ice"
                    },
                    new Category()
                    {
                        Name = "Fighting"
                    },
                    new Category()
                    {
                        Name = "Poison"
                    },
                    new Category()
                    {
                        Name = "Ground"
                    },
                    new Category()
                    {
                        Name = "Flying"
                    },
                    new Category()
                    {
                        Name = "Psychic"
                    },
                    new Category()
                    {
                        Name = "Bug"
                    },
                    new Category()
                    {
                        Name = "Rock"
                    },
                    new Category()
                    {
                        Name = "Ghost"
                    },
                    new Category()
                    {
                        Name = "Dragon"
                    },
                    new Category()
                    {
                        Name = "Dark"
                    },
                    new Category()
                    {
                        Name = "Steel"
                    },
                    new Category()
                    {
                        Name = "Fairy"
                    },
                    new Category()
                    {
                        Name = "Stellar"
                    }
                };

            Random random = new Random();

            List<PokemonCategory> pokemonCategories = new List<PokemonCategory>();

            for (int i = 1; i <= 50; i++)
            {
                pokemonCategories.Add(new PokemonCategory()
                {
                    PokemonId = i,
                    CategoryId = random.Next(1, 20)
                });
            }

            var ownersAndCountry = new List<Owner>()
            {
                // Kanto Region
                new Owner()
                {
                    FirstName = "Misty",
                    LastName = "",
                    Gym = "Cerulean Gym",
                    Country = new Country()
                    {
                        Name = "Kanto"
                    }
                },

                // Johto Region
                new Owner()
                {
                    FirstName = "Whitney",
                    LastName = "",
                    Gym = "Goldenrod Gym",
                    Country = new Country()
                    {
                        Name = "Johto"
                    }
                },

                // Hoenn Region
                new Owner()
                {
                    FirstName = "Steven",
                    LastName = "Stone",
                    Gym = "Elite Four",
                    Country = new Country()
                    {
                        Name = "Hoenn"
                    }
                },

                // Sinnoh Region
                new Owner()
                {
                    FirstName = "Cynthia",
                    LastName = "",
                    Gym = "Elite Four",
                    Country = new Country()
                    {
                        Name = "Sinnoh"
                    }
                },

                // Unova Region
                new Owner()
                {
                    FirstName = "Alder",
                    LastName = "",
                    Gym = "Elite Four",
                    Country = new Country()
                    {
                        Name = "Unova"
                    }
                },

                // Kalos Region
                new Owner()
                {
                    FirstName = "Diantha",
                    LastName = "",
                    Gym = "Elite Four",
                    Country = new Country()
                    {
                        Name = "Kalos"
                    }
                },

                // Alola Region
                new Owner()
                {
                    FirstName = "Professor",
                    LastName = "Kukui",
                    Gym = "Elite Four",
                    Country = new Country()
                    {
                        Name = "Alola"
                    }
                },

                // Galar Region
                new Owner()
                {
                    FirstName = "Leon",
                    LastName = "",
                    Gym = "Champion Stadium",
                    Country = new Country()
                    {
                        Name = "Galar"
                    }
                },

                // Paldea Region
                new Owner()
                {
                    FirstName = "Geeta",
                    LastName = "",
                    Gym = "Elite Four",
                    Country = new Country()
                    {
                        Name = "Paldea"
                    }
                },
            };

            List<PokemonOwner> pokemonOwners = new List<PokemonOwner>();

            for (int i = 1; i <= 50; i++)
            {
                pokemonOwners.Add(new PokemonOwner()
                {
                    PokemonId = i,
                    OwnerId = random.Next(1, 10)
                });
            }

            dataContext.Pokemon.AddRange(pokemons);
            dataContext.Categories.AddRange(categories);
            dataContext.Owners.AddRange(ownersAndCountry);
            dataContext.SaveChanges();
            dataContext.PokemonCategories.AddRange(pokemonCategories);
            dataContext.PokemonOwners.AddRange(pokemonOwners);
            dataContext.SaveChanges();
        }
    }
}
