using MediatR;
using Microsoft.AspNetCore.Mvc;
using SupportDesk.Application.Commands.Tickets;
using SupportDesk.Application.DTOs.Comments;
using SupportDesk.Application.DTOs.Tickets;
using SupportDesk.Application.Queries.Tickets;
using SupportDesk.Core.Specs;

namespace SupportDesk.API.Controllers;

public class TicketsController : ApiController
{
	private readonly IMediator _mediator;

	public TicketsController(IMediator mediator)
	{
		_mediator = mediator;
	}

	[HttpGet]
	public async Task<ActionResult<Pagination<TicketListItemDto>>> GetTickets(
		[FromQuery] TicketSpecParams ticketSpecParams,
		CancellationToken cancellationToken)
	{
		var query = new GetPaginatedTicketsQuery(ticketSpecParams);
		var result = await _mediator.Send(query, cancellationToken);
		return Ok(result);
	}

	[HttpGet("{id:guid}")]
	public async Task<ActionResult<TicketDetailDto>> GetTicketById([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		var result = await _mediator.Send(new GetTicketByIdQuery(id), cancellationToken);
		return result is null ? NotFound() : Ok(result);
	}

	[HttpPost]
	public async Task<ActionResult<TicketDetailDto>> CreateTicket(
		[FromBody] CreateTicketDto createTicketDto,
		CancellationToken cancellationToken)
	{
		var result = await _mediator.Send(new CreateTicketCommand(createTicketDto), cancellationToken);
		return CreatedAtAction(nameof(GetTicketById), new { id = result.Id }, result);
	}

	[HttpPut("{id:guid}")]
	public async Task<ActionResult<TicketDetailDto>> UpdateTicket(
		[FromRoute] Guid id,
		[FromBody] UpdateTicketDto updateTicketDto,
		CancellationToken cancellationToken)
	{
		var result = await _mediator.Send(new UpdateTicketCommand(id, updateTicketDto), cancellationToken);
		return Ok(result);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteTicket([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		var deleted = await _mediator.Send(new DeleteTicketCommand(id), cancellationToken);
		return deleted ? NoContent() : NotFound();
	}

	[HttpPut("{id:guid}/status")]
	public async Task<ActionResult<TicketDetailDto>> ChangeStatus(
		[FromRoute] Guid id,
		[FromBody] ChangeTicketStatusDto changeTicketStatusDto,
		CancellationToken cancellationToken)
	{
		var result = await _mediator.Send(new ChangeTicketStatusCommand(id, changeTicketStatusDto), cancellationToken);
		return Ok(result);
	}

	[HttpPut("{id:guid}/assign")]
	public async Task<ActionResult<TicketDetailDto>> AssignAgent(
		[FromRoute] Guid id,
		[FromBody] AssignAgentDto assignAgentDto,
		CancellationToken cancellationToken)
	{
		var result = await _mediator.Send(new AssignAgentCommand(id, assignAgentDto.AgentId), cancellationToken);
		return Ok(result);
	}

	[HttpDelete("{id:guid}/assign")]
	public async Task<ActionResult<TicketDetailDto>> UnassignAgent([FromRoute] Guid id, CancellationToken cancellationToken)
	{
		var result = await _mediator.Send(new UnassignAgentCommand(id), cancellationToken);
		return Ok(result);
	}

	[HttpPost("{id:guid}/comments")]
	public async Task<ActionResult<CommentDto>> AddComment(
		[FromRoute] Guid id,
		[FromBody] CreateCommentDto createCommentDto,
		CancellationToken cancellationToken)
	{
		var result = await _mediator.Send(new AddCommentCommand(id, createCommentDto), cancellationToken);
		return StatusCode(StatusCodes.Status201Created, result);
	}
}