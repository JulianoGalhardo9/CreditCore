namespace RuleEngine.Domain.Entities;

public record Score
{
    public int Value { get; init; }
    public string Description { get; init; }

    public Score(int value)
    {
        if (value < 0 || value > 1000)
            throw new ArgumentException("Score deve estar entre 0 e 1000.");

        Value = value;
        Description = value switch
        {
            < 300 => "Baixo",
            < 700 => "Médio",
            _ => "Alto"
        };
    }
}