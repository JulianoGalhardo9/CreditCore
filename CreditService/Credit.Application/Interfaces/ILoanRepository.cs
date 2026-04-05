using Credit.Domain.Entities;

namespace Credit.Application.Interfaces;

public interface ILoanRepository
{
    Task AddAsync(Loan loan);
    Task<Loan?> GetByIdAsync(Guid id);
}