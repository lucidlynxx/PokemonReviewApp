using Microsoft.EntityFrameworkCore;
using PokemonReviewApp.Data;
using PokemonReviewApp.Interfaces;
using PokemonReviewApp.Models;
using PokemonReviewApp.Dto;

namespace PokemonReviewApp.Repository;

public class ReviewerRepository : IReviewerRepository
{
    private readonly DataContext _context;

    public ReviewerRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateReviewer(Reviewer reviewer)
    {
        _context.Add(reviewer);
        return await Save();
    }

    public async Task<bool> DeleteReviewer(Reviewer reviewer)
    {
        _context.Remove(reviewer);
        return await Save();
    }

    public async Task<Reviewer?> GetReviewer(int reviewerId)
    {
        return await _context.Reviewers.Where(r => r.Id == reviewerId)
        .Include(e => e.Reviews)
        .FirstOrDefaultAsync()!;
    }

    public async Task<ICollection<Reviewer>> GetReviewers()
    {
        return await _context.Reviewers.ToListAsync();
    }

    public async Task<ICollection<Review>> GetReviewsByReviewer(int reviewerId)
    {
        return await _context.Reviews.Where(r => r.Reviewer!.Id == reviewerId).ToListAsync();
    }

    public async Task<bool> ReviewerExists(int reviewerId)
    {
        return await _context.Reviewers.AnyAsync(r => r.Id == reviewerId);
    }

    public async Task<bool> Save()
    {
        var saved = await _context.SaveChangesAsync();
        return saved > 0 ? true : false;
    }

    public async Task<bool> UpdateReviewer(Reviewer reviewer)
    {
        _context.Update(reviewer);
        return await Save();
    }

    public async Task<Reviewer> GetReviewerTrimToUpper(ReviewerDto reviewerDto)
    {
        var reviewers = await GetReviewers();

        return reviewers.Where(c => c.FirstName?.Trim().ToUpper() == reviewerDto?.FirstName?.TrimEnd().ToUpper())
            .FirstOrDefault()!;
    }
}
