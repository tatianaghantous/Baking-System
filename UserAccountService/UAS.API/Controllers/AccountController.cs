using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using UAS.Application.Features.Account.Commands;
using UAS.Application.Features.Account.Queries;

using UAS.Domain.Entities;

namespace UAS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController : ApiControllerBase
{
    
    [HttpGet, EnableQuery]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(await Mediator.Send(new GetOdataQuery()
        {
            Type = typeof(Account)
        }));
    }
    
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetAccountById(int id)
    {
        var result = await Mediator.Send(new GetAccountByIdQuery { AccountId = id });
        return Ok(result);
    }

    // Create a new account
    [HttpPost]
    [Authorize(Roles = "Admin, Employee")]
    public async Task<IActionResult> CreateAccount([FromBody] CreateAccountCommand command)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await Mediator.Send(command);
        return CreatedAtAction(nameof(GetAccountById), new { id = result.Id }, result);
    }
    [HttpPost("Update()")]
    [Authorize(Roles = "Employee,Admin")]
    public async Task<IActionResult> Update([FromBody] UpdateAccountCommand command)
    {

        var result = await Mediator.Send(command);

        if (!result)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteAccount(int id)
    {
        var result = await Mediator.Send(new DeleteAccountCommand { AccountId = id });
        if (!result)
            return NotFound();

        return NoContent();
    }
        
}