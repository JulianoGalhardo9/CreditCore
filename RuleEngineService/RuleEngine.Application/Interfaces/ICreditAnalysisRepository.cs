using RuleEngine.Domain.Entities;

namespace RuleEngine.Application.Interfaces;

public interface ICreditAnalysisRepository
{
    Task AddAsync(CreditAnalysis analysis);
}