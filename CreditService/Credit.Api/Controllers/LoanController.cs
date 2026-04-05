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

    //Avaliar o Empréstimo
    [HttpPost("evaluate")]
    public async Task<IActionResult> EvaluateLoan([FromBody] EvaluateLoanCommand command)
    {
        try
        {
            await _mediator.Send(command);

            return Ok(new { message = "Avaliação concluída com sucesso. O status do empréstimo foi atualizado!" });
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}