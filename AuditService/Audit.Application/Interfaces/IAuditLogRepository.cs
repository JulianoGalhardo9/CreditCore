namespace Audit.Application.Interfaces;

public interface IAuditLogRepository
{
    Task AddAsync(AuditLog log);
}