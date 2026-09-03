using SupportDesk.Core.Enums;

namespace SupportDesk.Application.DTOs.Agents;

public record UpdateAgentDto(
	string FullName,
	string Email,
	Department Department,
	bool IsActive);
