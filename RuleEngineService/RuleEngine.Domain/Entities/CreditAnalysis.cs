namespace RuleEngine.Domain.Entities;

public class CreditAnalysis
{
    public Guid Id { get; private set; }
    public Guid LoanId { get; private set; }
    public Score Score { get; private set; }
    public bool Approved { get; private set; }
    public string Observation { get; private set; }

    public CreditAnalysis(Guid loanId, int scoreValue)
    {
        Id = Guid.NewGuid();
        LoanId = loanId;
        Score = new Score(scoreValue);
        Approved = Score.Value > 600;
        Observation = Approved ? "Aprovado automaticamente pelo motor" : "Reprovado por score insuficiente";
    }
}