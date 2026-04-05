using MediatR;

namespace Credit.Application.Commands;
public record EvaluateLoanCommand(Guid LoanId, bool IsApproved) : IRequest<bool>;