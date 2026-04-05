using Credit.Domain.Enums;

namespace Credit.Domain.Entities;

public class Loan
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public decimal Amount { get; private set; }
    public int Installments { get; private set; }
    public LoanStatus Status { get; private set; }
    
    public DateTime CreatedAt { get; private set; }

    public Loan(Guid userId, decimal amount, int installments)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        Amount = amount;
        Installments = installments;
        Status = LoanStatus.Pending; 
        CreatedAt = DateTime.UtcNow;
    }
    public void Approve()
    {
        Status = LoanStatus.Approved;
    }
    public void Reject()
    {
        Status = LoanStatus.Rejected;
    }
}