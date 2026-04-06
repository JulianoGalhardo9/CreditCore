namespace RuleEngine.Application.Events;

public record CreditEvaluatedEvent(Guid LoanId, bool Approved, string Observation, int Score);