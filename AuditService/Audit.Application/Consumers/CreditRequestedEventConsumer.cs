using System.Text.Json;
using MassTransit;
using Credit.Application.Events; // O nosso namespace "truque" para o endereço bater
using Audit.Domain.Entities;
using Audit.Application.Interfaces;

namespace Audit.Application.Consumers;
public class CreditRequestedEventConsumer : IConsumer<CreditRequestedEvent>
{
    private readonly IAuditLogRepository _repository;

    public CreditRequestedEventConsumer(IAuditLogRepository repository)
    {
        _repository = repository;
    }

    public async Task Consume(ConsumeContext<CreditRequestedEvent> context)
    {

        var message = context.Message;

        var payloadJson = JsonSerializer.Serialize(message);

        var auditLog = new AuditLog("CreditRequested", payloadJson);

        await _repository.AddAsync(auditLog);
    }
}