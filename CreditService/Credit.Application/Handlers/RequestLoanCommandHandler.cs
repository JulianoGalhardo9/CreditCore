using MediatR;
using Credit.Application.Commands;
using Credit.Application.Interfaces;
using Credit.Domain.Entities;

namespace Credit.Application.Handlers;

// O Handler recebe o comando e devolve o Guid (o protocolo gerado).
public class RequestLoanCommandHandler : IRequestHandler<RequestLoanCommand, Guid>
{
    private readonly ILoanRepository _loanRepository;

    public RequestLoanCommandHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<Guid> Handle(RequestLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = new Loan(request.UserId, request.Amount, request.Installments);

        await _loanRepository.AddAsync(loan);

        return loan.Id;
    }
}