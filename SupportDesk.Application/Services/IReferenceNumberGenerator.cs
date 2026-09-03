namespace SupportDesk.Application.Services;

public interface IReferenceNumberGenerator
{
	Task<string> GenerateAsync(CancellationToken cancellationToken);
}

