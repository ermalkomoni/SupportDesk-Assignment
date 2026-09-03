using SupportDesk.Core.Entities;

namespace SupportDesk.Core.Repositories;

public interface IAgentRepository
{
	Task<IEnumerable<Agent>> GetAllAgents(string? search, CancellationToken cancellationToken);

	Task<Agent?> GetAgentById(Guid id, CancellationToken cancellationToken);

	Task<Agent?> GetTrackedAgentById(Guid id, CancellationToken cancellationToken);

	Task<bool> EmailExists(string email, Guid? excludeId, CancellationToken cancellationToken);

	Task<bool> HasAssignedTickets(Guid agentId, CancellationToken cancellationToken);

	Task AddAgent(Agent agent, CancellationToken cancellationToken);

	Task UpdateAgent(Agent agent, CancellationToken cancellationToken);

	Task DeleteAgent(Agent agent, CancellationToken cancellationToken);
}

