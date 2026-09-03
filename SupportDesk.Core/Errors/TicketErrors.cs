namespace SupportDesk.Core.Errors;

public static class TicketErrors
{
	public const string Closed = "This ticket is closed and can no longer be modified.";

	public const string InProgressRequiresActiveAgent =
		"A ticket can only move to In Progress once an active agent is assigned.";

	public const string CannotUnassignInProgress =
		"An in-progress ticket must always have an active agent; move it back to New or reassign it instead of unassigning.";

	public static string InactiveAgent(string agentName) =>
		$"Agent '{agentName}' is inactive and cannot be assigned tickets.";
}
