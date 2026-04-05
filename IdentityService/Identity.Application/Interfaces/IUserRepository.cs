using Identity.Domain.Entities;

namespace Identity.Application.Interfaces;

public interface IUserRepository
{
    Task AddAsync(User user);
    Task<bool> ExistsByEmailAsync(string email);

    Task<User?> GetByEmailAsync(string email); 
}