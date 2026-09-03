using SupportDesk.Core.Entities;
using SupportDesk.Core.Specs;

namespace SupportDesk.Core.Repositories;

public interface ITicketRepository
{
	Task<Pagination<Ticket>> GetPaginatedTickets(TicketSpecParams ticketSpecParams, CancellationToken cancellationToken);

	Task<Ticket?> GetTicketById(Guid id, CancellationToken cancellationToken);

	Task<Ticket?> GetTrackedTicketById(Guid id, CancellationToken cancellationToken);

	Task<bool> ReferenceExists(string reference, CancellationToken cancellationToken);

	Task<int> CountCreatedInYear(int year, CancellationToken cancellationToken);

	Task AddTicket(Ticket ticket, CancellationToken cancellationToken);

	Task UpdateTicket(Ticket ticket, CancellationToken cancellationToken);

	Task DeleteTicket(Ticket ticket, CancellationToken cancellationToken);
}