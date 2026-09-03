using Microsoft.EntityFrameworkCore;
using SupportDesk.Core.Entities;
using SupportDesk.Core.Enums;
using SupportDesk.Core.Repositories;
using SupportDesk.Core.Specs;
using SupportDesk.Infrastructure.Data;

namespace SupportDesk.Infrastructure.Repositories;

public class TicketRepository : ITicketRepository
{
	private readonly ApplicationDbContext _context;

	public TicketRepository(ApplicationDbContext context)
	{
		ArgumentNullException.ThrowIfNull(context, nameof(context));
		_context = context;
	}

	public async Task<Pagination<Ticket>> GetPaginatedTickets(TicketSpecParams ticketSpecParams, CancellationToken cancellationToken)
	{
		ArgumentNullException.ThrowIfNull(ticketSpecParams, nameof(ticketSpecParams));

		var query = _context.Tickets
							.Include(t => t.AssignedAgent)
							.AsNoTracking()
							.AsQueryable();

		query = ApplyFilters(query, ticketSpecParams);

		var totalCount = await query.CountAsync(cancellationToken);

		var items = await query
			.OrderByDescending(t => t.CreatedDate)
			.Skip(ticketSpecParams.Skip)
			.Take(ticketSpecParams.Take)
			.ToListAsync(cancellationToken);

		var totalPages = (int)Math.Ceiling(totalCount / (double)ticketSpecParams.PageSize);

		return new Pagination<Ticket>(items, ticketSpecParams.PageNumber, ticketSpecParams.PageSize, totalCount, totalPages);
	}

	public async Task<Ticket?> GetTicketById(Guid id, CancellationToken cancellationToken)
	{
		return await _context.Tickets
			.Include(t => t.AssignedAgent)
			.Include(t => t.Comments)
			.AsNoTracking()
			.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
	}

	public async Task<Ticket?> GetTrackedTicketById(Guid id, CancellationToken cancellationToken)
	{
		return await _context.Tickets
			.Include(t => t.AssignedAgent)
			.Include(t => t.Comments)
			.FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
	}

	public Task<bool> ReferenceExists(string reference, CancellationToken cancellationToken) =>
		_context.Tickets.AsNoTracking().AnyAsync(t => t.Reference == reference, cancellationToken);

	public Task<int> CountCreatedInYear(int year, CancellationToken cancellationToken) =>
		_context.Tickets.AsNoTracking().CountAsync(t => t.CreatedDate.Year == year, cancellationToken);

	public async Task AddTicket(Ticket ticket, CancellationToken cancellationToken)
	{
		_context.Tickets.Add(ticket);
		await _context.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateTicket(Ticket ticket, CancellationToken cancellationToken)
	{
		await _context.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteTicket(Ticket ticket, CancellationToken cancellationToken)
	{
		_context.Tickets.Remove(ticket);
		await _context.SaveChangesAsync(cancellationToken);
	}

	private static IQueryable<Ticket> ApplyFilters(IQueryable<Ticket> query, TicketSpecParams parameters)
	{
		if (!string.IsNullOrWhiteSpace(parameters.Search))
		{
			var term = $"%{parameters.Search.Trim()}%";
			query = query.Where(t =>
				EF.Functions.ILike(t.Reference, term) ||
				EF.Functions.ILike(t.Title, term) ||
				EF.Functions.ILike(t.CustomerName, term));
		}

		if (parameters.Status is { } status)
			query = query.Where(t => t.Status == status);

		if (parameters.Priority is { } priority)
			query = query.Where(t => t.Priority == priority);

		if (parameters.AssignedAgentId is { } agentId)
			query = query.Where(t => t.AssignedAgentId == agentId);

		if (parameters.OverdueOnly)
		{
			var now = DateTime.UtcNow;
			query = query.Where(t =>
				t.Status != TicketStatus.Resolved &&
				t.Status != TicketStatus.Closed &&
				t.DueDate < now);
		}

		return query;
	}
}

