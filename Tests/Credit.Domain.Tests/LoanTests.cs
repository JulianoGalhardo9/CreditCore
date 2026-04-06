using Credit.Domain.Entities;
using Credit.Domain.Enums; // Importante para reconhecer o LoanStatus
using FluentAssertions;

namespace Credit.Domain.Tests;

public class LoanTests
{
    [Fact]
    public void Approve_ShouldChangeStatusToApproved()
    {
        // 1. Arrange 
        var customerId = Guid.NewGuid();
        var loan = new Loan(customerId, 5000m, 12); 

        // 2. Act
        loan.Approve();

        // 3. Assert
        loan.Status.Should().Be(LoanStatus.Approved); 
    }

    [Fact]
    public void Reject_ShouldChangeStatusToRejected()
    {
        // 1. Arrange
        var customerId = Guid.NewGuid();
        var loan = new Loan(customerId, 10000m, 24);

        // 2. Act
        loan.Reject();

        // 3. Assert
        loan.Status.Should().Be(LoanStatus.Rejected);
    }
}