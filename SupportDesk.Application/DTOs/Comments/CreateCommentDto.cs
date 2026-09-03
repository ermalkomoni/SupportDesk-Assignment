namespace SupportDesk.Application.DTOs.Comments;

public record CreateCommentDto(
	string AuthorName,
	string Body);

