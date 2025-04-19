using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using UAS.Application.Features.User.Commands;
using UAS.Application.Features.User.Queries;
using UAS.Domain.Entities;

namespace UAS.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ApiControllerBase
{
    [HttpGet, EnableQuery]
    [Authorize]
    public async Task<ActionResult> Get()
    {
        return Ok(await Mediator.Send(new GetOdataQuery()
        {
            Type = typeof(User)
        }));
    }
    
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var userDto = await Mediator.Send(command);
        
        return CreatedAtAction(nameof(GetUserById), new { id = userDto.Id }, userDto);
    }
    [HttpGet("{id}")]
    [Authorize(Roles = "Admin, Employee")]
    public async Task<IActionResult> GetUserById(Guid id)
    {
        
        var query = new GetUserByIdQuery { UserId = id };
        var user = await Mediator.Send(query);
        if (user != null)
        {
            return Ok(user);
        }
        return NotFound();
    }
}