using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Dto;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;

namespace PokemonReviewApp.Repository;

public class UserRepository : IUserRepository
{
    private readonly DataContext _context;

    public UserRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateUser(User user)
    {
        _context.Add(user);
        return await Save();
    }

    public async Task<bool> DeleteUser(User user)
    {
        _context.Remove(user);
        return await Save();
    }

    public async Task<User?> GetUserById(int id)
    {
        return await _context.Users.Where(u => u.Id == id).FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByIdNoTracking(int id)
    {
        return await _context.Users.Where(u => u.Id == id).AsNoTracking().FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await _context.Users.Where(u => u.Username == username).FirstOrDefaultAsync();
    }

    public async Task<ICollection<User>> GetUsers()
    {
        return await _context.Users.ToListAsync();
    }

    public async Task<ICollection<User>> GetUsersForPagedResponse(int pageNumber, int validFilterPageSize)
    {
        return await _context.Users.Skip((pageNumber - 1) * validFilterPageSize)
            .Take(validFilterPageSize)
            .ToListAsync();
    }

    public async Task<User> GetUserTrimToUpper(CreateUserDto createUserDto)
    {
        User? user = await GetUserByUsername(createUserDto.Username!);

        if (user == null)
            return null!;

        if (user?.Username?.Trim().ToUpper() == createUserDto.Username?.Trim().ToUpper())
            return user!;

        return null!;
    }

    public async Task<bool> Save()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0 ? true : false;
    }

    public async Task<bool> UpdateUser(User user)
    {
        _context.Update(user);
        return await Save();
    }

    public async Task<bool> UserExistsById(int id)
    {
        return await _context.Users.AnyAsync(u => u.Id == id);
    }

    public async Task<bool> UserExistsByUsername(string username)
    {
        return await _context.Users.AnyAsync(u => u.Username == username);
    }

    public async Task<bool> UserExistsByEmail(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
}
