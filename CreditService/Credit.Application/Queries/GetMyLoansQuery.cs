using MediatR;
using Credit.Domain.Entities;

namespace Credit.Application.Queries;
public record GetMyLoansQuery(Guid UserId) : IRequest<IEnumerable<Loan>>;