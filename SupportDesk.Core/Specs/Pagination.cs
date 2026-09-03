namespace SupportDesk.Core.Specs;

public class Pagination<T> where T : class
{
	public IReadOnlyList<T> Items { get; }
	public int PageNumber { get; }
	public int PageSize { get; }
	public int TotalCount { get; }
	public int TotalPages { get; }
	public bool HasPrevious => PageNumber > 1;
	public bool HasNext => PageNumber < TotalPages;

	public Pagination(IReadOnlyList<T> items, int pageNumber, int pageSize, int totalCount, int totalPages)
	{
		Items = items;
		PageNumber = pageNumber;
		PageSize = pageSize;
		TotalCount = totalCount;
		TotalPages = totalPages;
	}
}