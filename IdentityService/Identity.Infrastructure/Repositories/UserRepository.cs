using Microsoft.EntityFrameworkCore;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;
using Identity.Infrastructure.Data;

namespace Identity.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;

    public UserRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> ExistsByEmailAsync(string email)
    {
        return await _context.Users.AnyAsync(u => u.Email == email);
    }
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }
}