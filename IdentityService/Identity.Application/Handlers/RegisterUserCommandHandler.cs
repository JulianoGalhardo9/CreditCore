using MediatR;
using Identity.Application.Commands;
using Identity.Application.Interfaces;
using Identity.Domain.Entities;

namespace Identity.Application.Handlers;
public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    public RegisterUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }
    public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        bool emailExists = await _userRepository.ExistsByEmailAsync(request.Email);
        
        if (emailExists)
        {
            throw new Exception("Este e-mail já está em uso.");
        }

        string hashedPassword = _passwordHasher.Hash(request.Password);

        var user = new User(
            request.FullName, 
            request.Email, 
            hashedPassword,
            request.Role
        );

        await _userRepository.AddAsync(user);

        return user.Id;
    }
}