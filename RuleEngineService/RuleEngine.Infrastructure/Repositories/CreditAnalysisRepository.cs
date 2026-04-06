using RuleEngine.Application.Interfaces;
using RuleEngine.Domain.Entities;
using RuleEngine.Infrastructure.Data;

namespace RuleEngine.Infrastructure.Repositories;

public class CreditAnalysisRepository : ICreditAnalysisRepository
{
    private readonly AppDbContext _context;

    public CreditAnalysisRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(CreditAnalysis analysis)
    {
        await _context.CreditAnalyses.AddAsync(analysis);
        await _context.SaveChangesAsync();
    }
}