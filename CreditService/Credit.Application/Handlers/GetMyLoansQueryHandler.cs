using MediatR;
using Credit.Application.Queries;
using Credit.Application.Interfaces;
using Credit.Domain.Entities;

namespace Credit.Application.Handlers;
public class GetMyLoansQueryHandler : IRequestHandler<GetMyLoansQuery, IEnumerable<Loan>>
{
    private readonly ILoanRepository _loanRepository;

    public GetMyLoansQueryHandler(ILoanRepository loanRepository)
    {
        _loanRepository = loanRepository;
    }

    public async Task<IEnumerable<Loan>> Handle(GetMyLoansQuery request, CancellationToken cancellationToken)
    {
        var myLoans = await _loanRepository.GetByUserIdAsync(request.UserId);

        return myLoans;
    }
}