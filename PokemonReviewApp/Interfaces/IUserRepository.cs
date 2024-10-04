using PokemonReviewApp.Dto;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Interfaces;

public interface IUserRepository
{
    Task<ICollection<User>> GetUsers();
    Task<ICollection<User>> GetUsersForPagedResponse(int pageNumber, int validFilterPageSize);
    Task<User?> GetUserById(int id);
    Task<User?> GetUserByIdNoTracking(int id);
    Task<User?> GetUserByUsername(string username);
    Task<User> GetUserTrimToUpper(CreateUserDto createUserDto);
    Task<bool> UserExistsById(int id);
    Task<bool> UserExistsByUsername(string username);
    Task<bool> UserExistsByEmail(string email);
    Task<bool> CreateUser(User user);
    Task<bool> UpdateUser(User user);
    Task<bool> DeleteUser(User user);
    Task<bool> Save();
}
