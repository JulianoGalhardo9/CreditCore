using MediatR;
using MassTransit; // NOVO: A importação do nosso carteiro
using Credit.Application.Commands;
using Credit.Application.Interfaces;
using Credit.Application.Events;
using Credit.Domain.Entities;

namespace Credit.Application.Handlers;

public class RequestLoanCommandHandler : IRequestHandler<RequestLoanCommand, Guid>
{
    private readonly ILoanRepository _loanRepository;
    private readonly IPublishEndpoint _publishEndpoint;
    public RequestLoanCommandHandler(ILoanRepository loanRepository, IPublishEndpoint publishEndpoint)
    {
        _loanRepository = loanRepository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<Guid> Handle(RequestLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = new Loan(request.UserId, request.Amount, request.Installments);

        await _loanRepository.AddAsync(loan);
        
        var loanEvent = new CreditRequestedEvent(loan.Id, loan.UserId, loan.Amount, loan.Installments);
        await _publishEndpoint.Publish(loanEvent, cancellationToken);

        return loan.Id;
    }
}