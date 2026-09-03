using MediatR;
using Microsoft.AspNetCore.Mvc;
using SupportDesk.Application.Commands.Agents;
using SupportDesk.Application.DTOs.Agents;
using SupportDesk.Application.Queries.Agents;

namespace SupportDesk.API.Controllers;

public class AgentsController : ApiController
{
	private readonly IMediator _mediator;

	public AgentsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public async Task<ActionResult<IEnumerable<AgentDto>>> GetAgents(
		[FromQuery] string? search,
		CancellationToken cancellationToken)
	{
		var result = await _mediator.Send(new GetAllAgentsQuery(search), cancellationToken);
		return Ok(result);
	}

	[HttpGet("{id:guid}")]
	public async Task<ActionResult<AgentDto>> GetAgentById([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		var result = await _mediator.Send(new GetAgentByIdQuery(id), cancellationToken);
		return result is null ? NotFound() : Ok(result);
	}

	[HttpPost]
	public async Task<ActionResult<AgentDto>> CreateAgent(
		[FromBody] CreateAgentDto createAgentDto,
		CancellationToken cancellationToken)
	{
		var result = await _mediator.Send(new CreateAgentCommand(createAgentDto), cancellationToken);
		return CreatedAtAction(nameof(GetAgentById), new { id = result.Id }, result);
	}

	[HttpPut("{id:guid}")]
	public async Task<ActionResult<AgentDto>> UpdateAgent(
		[FromRoute] Guid id,
		[FromBody] UpdateAgentDto updateAgentDto,
		CancellationToken cancellationToken)
	{
		var result = await _mediator.Send(new UpdateAgentCommand(id, updateAgentDto), cancellationToken);
		return Ok(result);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteAgent([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		var deleted = await _mediator.Send(new DeleteAgentCommand(id), cancellationToken);
		return deleted ? NoContent() : NotFound();
	}
}
