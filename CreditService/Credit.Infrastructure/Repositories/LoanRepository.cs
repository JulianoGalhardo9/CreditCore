using Credit.Application.Interfaces;
using Credit.Domain.Entities;
using Credit.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Credit.Infrastructure.Repositories;

public class LoanRepository : ILoanRepository
{
    private readonly AppDbContext _context;

    public LoanRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Loan loan)
    {
        await _context.Loans.AddAsync(loan);
        await _context.SaveChangesAsync();
    }

    public async Task<Loan?> GetByIdAsync(Guid id)
    {
        return await _context.Loans.FindAsync(id);
    }

    public async Task UpdateAsync(Loan loan)
    {
        _context.Loans.Update(loan);
        await _context.SaveChangesAsync();
    }
}