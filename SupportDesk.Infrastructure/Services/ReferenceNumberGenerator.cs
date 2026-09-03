using SupportDesk.Application.Services;
using SupportDesk.Core.Repositories;

namespace SupportDesk.Infrastructure.Services;

public class ReferenceNumberGenerator : IReferenceNumberGenerator
{
	private const int MaxAttempts = 5;
	private readonly ITicketRepository _ticketRepository;

	public ReferenceNumberGenerator(ITicketRepository ticketRepository)
	{
		_ticketRepository = ticketRepository;
	}

	public async Task<string> GenerateAsync(CancellationToken cancellationToken)
	{
		var year = DateTime.UtcNow.Year;

		for (var attempt = 0; attempt < MaxAttempts; attempt++)
		{
			var nextSequence = await _ticketRepository.CountCreatedInYear(year, cancellationToken) + 1 + attempt;
			var candidate = $"TCK-{year}-{nextSequence:D4}";

			if (!await _ticketRepository.ReferenceExists(candidate, cancellationToken))
				return candidate;
		}

		return $"TCK-{year}-{Guid.NewGuid():N}"[..17];
	}
}
