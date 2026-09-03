namespace SupportDesk.Application.DTOs.Comments;

public record CommentDto(
	Guid Id,
	string AuthorName,
	string Body,
	DateTime CreatedDate
);
