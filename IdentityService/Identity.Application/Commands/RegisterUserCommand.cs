using MediatR;
using Identity.Domain.Enums;

namespace Identity.Application.Commands;
public record RegisterUserCommand(
    
    string FullName, 
    
    string Email, 
    
    string Password, 
    
    Role Role

) : IRequest<Guid>;