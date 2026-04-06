namespace Credit.Application.Events;
public record CreditRequestedEvent(Guid LoanId, Guid UserId, decimal Amount, int Installments);