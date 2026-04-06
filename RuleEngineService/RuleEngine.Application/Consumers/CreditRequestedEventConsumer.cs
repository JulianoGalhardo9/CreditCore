using MassTransit;
using Credit.Application.Events;
using RuleEngine.Application.Events;
using RuleEngine.Application.Interfaces;
using RuleEngine.Domain.Entities;

namespace RuleEngine.Application.Consumers;
public class CreditRequestedEventConsumer : IConsumer<CreditRequestedEvent>
{
    private readonly ICreditAnalysisRepository _repository;
    private readonly IPublishEndpoint _publishEndpoint;
    public CreditRequestedEventConsumer(
        ICreditAnalysisRepository repository, 
        IPublishEndpoint publishEndpoint)
    {
        _repository = repository;
        _publishEndpoint = publishEndpoint;
    }

    public async Task Consume(ConsumeContext<CreditRequestedEvent> context)
    {
        var message = context.Message;

        var randomScore = new Random().Next(100, 1000);

        var analysis = new CreditAnalysis(message.LoanId, randomScore);

        await _repository.AddAsync(analysis);

        var evaluatedEvent = new CreditEvaluatedEvent(
            analysis.LoanId, 
            analysis.Approved, 
            analysis.Observation, 
            analysis.Score.Value
        );

        await _publishEndpoint.Publish(evaluatedEvent);
    }
}