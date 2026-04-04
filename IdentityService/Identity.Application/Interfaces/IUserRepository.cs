using Identity.Domain.Entities;

namespace Identity.Application.Interfaces;
public interface IUserRepository
{
    // Esta promessa diz: "Quem me usar, terá um método para adicionar um usuário".
    Task AddAsync(User user);
    Task<bool> ExistsByEmailAsync(string email);
}