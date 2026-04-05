using System.Security.Claims;
using Credit.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Credit.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize] 
public class LoanController : ControllerBase
{
    private readonly IMediator _mediator;
    public LoanController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> RequestLoan([FromBody] RequestLoanCommand command)
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userIdClaim))
        {
            return Unauthorized(new { error = "Crachá inválido ou sem identificação." });
        }

        command.UserId = Guid.Parse(userIdClaim);

        var loanId = await _mediator.Send(command);

        return Created("", new { loanId = loanId });
    }
}