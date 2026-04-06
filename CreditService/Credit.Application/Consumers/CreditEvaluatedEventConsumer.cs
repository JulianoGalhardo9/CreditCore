using MassTransit;
using Credit.Application.Interfaces;
using RuleEngine.Application.Events;

namespace Credit.Application.Consumers;

public class CreditEvaluatedEventConsumer : IConsumer<CreditEvaluatedEvent>
{
    private readonly ILoanRepository _repository;

    public CreditEvaluatedEventConsumer(ILoanRepository repository)
    {
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<CreditEvaluatedEvent> context)
    {
        var message = context.Message;

        var loan = await _repository.GetByIdAsync(message.LoanId);

        if (loan == null)
            return;

        if (message.Approved)
        {
            loan.Approve(); 
        }
        else
        {
            loan.Reject(); 
        }
        
        await _repository.UpdateAsync(loan);
    }
}