namespace Audit.Domain.Entities;

public class AuditLog
{
    public Guid Id { get; private set; }
    
    public string EventName { get; private set; }
    public string Payload { get; private set; }
    
    public DateTime OccurredAt { get; private set; }

    public AuditLog(string eventName, string payload)
    {
        Id = Guid.NewGuid();
        EventName = eventName;
        Payload = payload;
        OccurredAt = DateTime.UtcNow;
    }
}