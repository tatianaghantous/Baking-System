using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TS.Persistence.Features.Transaction;
using TS.Persistence.Features.Transaction.Commands;
using TS.Persistence.Features.Transaction.Queries;

namespace TS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TransactionController : ApiControllerBase
{
    [HttpPost]
    [Authorize(Roles = "Admin,Employee,Customer")]
    public async Task<IActionResult> AddTransaction([FromBody] CreateTransactionCommand command)
    {
        var result = await Mediator.Send(command);
        return Ok(result);
    }

    [HttpGet("{transactionId}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> GetTransactionById(int transactionId)
    {
        var result = await Mediator.Send(new GetTransactionByIdQuery { TransactionId = transactionId });
        return Ok(result);
    }
    
    [HttpGet("account/{accountId}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> GetTransactionsByAccount(int accountId)
    {
        var result = await Mediator.Send(new GetTransactionsByAccountQuery { AccountId = accountId });
        return Ok(result);
    }
    
    [HttpPut("{transactionId}")]
    [Authorize(Roles = "Admin,Employee")]
    public async Task<IActionResult> UpdateTransaction(int transactionId, [FromBody] UpdateTransactionCommand command)
    {
        if (transactionId != command.Id)
            return BadRequest();

        await Mediator.Send(command);
        return NoContent();
    }
    
}