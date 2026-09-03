using SupportDesk.Core.Enums;

namespace SupportDesk.Application.DTOs.Agents;

public record AgentDto(
	Guid Id,
	string FullName,
	string Email,
	Department Department,
	bool IsActive,
	int AssignedTicketCount
);