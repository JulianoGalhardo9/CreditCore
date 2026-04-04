using MediatR;

namespace Identity.Application.Commands;
public record LoginUserCommand(
    
    string Email, 
    
    string Password

) : IRequest<string>;