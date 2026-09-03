namespace SupportDesk.Core.Errors;

public static class AgentErrors
{
	public const string HasAssignedTickets =
		"This agent has tickets assigned and cannot be deleted. Deactivate the agent instead.";

	public static string EmailInUse(string email) =>
		$"An agent with email '{email}' already exists.";
}
