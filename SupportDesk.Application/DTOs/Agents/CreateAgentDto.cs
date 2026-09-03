using SupportDesk.Core.Enums;

namespace SupportDesk.Application.DTOs.Agents;

public record CreateAgentDto(
	string FullName,
	string Email,
	Department Department);
