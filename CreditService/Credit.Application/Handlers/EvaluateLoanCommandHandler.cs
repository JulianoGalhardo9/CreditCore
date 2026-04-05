using MediatR;
using Credit.Application.Commands;
using Credit.Application.Interfaces;

namespace Credit.Application.Handlers;

public class EvaluateLoanCommandHandler : IRequestHandler<EvaluateLoanCommand, bool>
{
    private readonly ILoanRepository _loanRepository;

    public EvaluateLoanCommandHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<bool> Handle(EvaluateLoanCommand request, CancellationToken cancellationToken)
    {
        var loan = await _loanRepository.GetByIdAsync(request.LoanId);

        if (loan == null)
        {
            throw new Exception("Empréstimo não encontrado no sistema.");
        }

        if (request.IsApproved)
        {
            loan.Approve(); 
        }
        else
        {
            loan.Reject();
        }

        await _loanRepository.UpdateAsync(loan);

        return true;
    }
}