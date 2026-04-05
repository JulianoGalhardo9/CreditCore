using System.Text.Json.Serialization;
using MediatR;

namespace Credit.Application.Commands;
public class RequestLoanCommand : IRequest<Guid>
{
    [JsonIgnore] 
    public Guid UserId { get; set; }
    public decimal Amount { get; set; }
    public int Installments { get; set; }
}