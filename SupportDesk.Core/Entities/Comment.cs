namespace SupportDesk.Core.Entities;

public class Comment : BaseEntity
{
	public string AuthorName { get; set; } = default!;
	public string Body { get; set; } = default!;
	public DateTime CreatedDate { get; set; }
	public Guid TicketId { get; set; }
	public Ticket Ticket { get; set; } = default!;
}