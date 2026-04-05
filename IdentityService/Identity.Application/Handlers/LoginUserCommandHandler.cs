using MediatR;
using Identity.Application.Commands;
using Identity.Application.Interfaces;

namespace Identity.Application.Handlers;

public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, string>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;

    public LoginUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService)
    {
        _userRepository = userRepository; 
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
    }

    public async Task<string> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        // 1. Tenta buscar o usuário no banco pelo e-mail fornecido.
        var user = await _userRepository.GetByEmailAsync(request.Email);

        if (user == null)
        {
            throw new Exception("E-mail ou senha incorretos."); // Mensagem genérica por segurança.
        }

        // 2. Verifica se a senha digitada, após ser "moída", é igual ao hash do banco.
        bool isPasswordValid = _passwordHasher.Verify(request.Password, user.PasswordHash);

        if (!isPasswordValid)
        {
            throw new Exception("E-mail ou senha incorretos."); 
        }

        // 3. Se passou em tudo, pedimos para o serviço gerar o "crachá" (Token).
        return _tokenService.GenerateToken(user);
    }
}