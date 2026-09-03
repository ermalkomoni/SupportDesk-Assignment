using Microsoft.EntityFrameworkCore;
using SupportDesk.Core.Entities;
using SupportDesk.Core.Repositories;
using SupportDesk.Infrastructure.Data;

namespace SupportDesk.Infrastructure.Repositories;

public class AgentRepository : IAgentRepository
{
	private readonly ApplicationDbContext _context;

	public AgentRepository(ApplicationDbContext context)
	{
		ArgumentNullException.ThrowIfNull(context, nameof(context));
		_context = context;
	}

	public async Task<IEnumerable<Agent>> GetAllAgents(string? search, CancellationToken cancellationToken)
	{
		var query = _context.Agents
							.Include(a => a.Tickets)
							.AsNoTracking()
							.AsQueryable();

		if (!string.IsNullOrWhiteSpace(search))
		{
			var term = $"%{search.Trim()}%";
			query = query.Where(a => EF.Functions.ILike(a.FullName, term) || EF.Functions.ILike(a.Email, term));
		}

		return await query
			.OrderBy(a => a.FullName)
			.ToListAsync(cancellationToken);
	}

	public async Task<Agent?> GetAgentById(Guid id, CancellationToken cancellationToken)
	{
		return await _context.Agents
			.Include(a => a.Tickets)
			.AsNoTracking()
			.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
	}

	public async Task<Agent?> GetTrackedAgentById(Guid id, CancellationToken cancellationToken)
	{
		return await _context.Agents.FirstOrDefaultAsync(a => a.Id == id, cancellationToken);
	}

	public Task<bool> EmailExists(string email, Guid? excludeId, CancellationToken cancellationToken)
	{
		var normalized = email.Trim().ToLower();
		return _context.Agents
			.AsNoTracking()
			.Where(a => a.Email == normalized && (excludeId == null || a.Id != excludeId))
			.AnyAsync(cancellationToken);
	}

	public Task<bool> HasAssignedTickets(Guid agentId, CancellationToken cancellationToken) =>
		_context.Tickets.AsNoTracking().AnyAsync(t => t.AssignedAgentId == agentId, cancellationToken);

	public async Task AddAgent(Agent agent, CancellationToken cancellationToken)
	{
		_context.Agents.Add(agent);
		await _context.SaveChangesAsync(cancellationToken);
	}

	public async Task UpdateAgent(Agent agent, CancellationToken cancellationToken)
	{
		await _context.SaveChangesAsync(cancellationToken);
	}

	public async Task DeleteAgent(Agent agent, CancellationToken cancellationToken)
	{
		_context.Agents.Remove(agent);
		await _context.SaveChangesAsync(cancellationToken);
	}
}

