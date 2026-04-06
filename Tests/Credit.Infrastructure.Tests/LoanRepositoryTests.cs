using Credit.Domain.Entities;
using Credit.Infrastructure.Data;
using Credit.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;

namespace Credit.Infrastructure.Tests;

public class LoanRepositoryTests
{
    private AppDbContext GetDatabase()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task AddAsync_ShouldPersistLoanInDatabase()
    {
        // Arrange
        var db = GetDatabase();
        var repository = new LoanRepository(db);
        var loan = new Loan(Guid.NewGuid(), 1500m, 6);

        // Act
        await repository.AddAsync(loan);

        // Assert
        var savedLoan = await db.Loans.FirstOrDefaultAsync();
        savedLoan.Should().NotBeNull();
        savedLoan!.Amount.Should().Be(1500m);
    }
}