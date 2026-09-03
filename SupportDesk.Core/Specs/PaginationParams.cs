namespace SupportDesk.Core.Specs;

public class PaginationParams
{
	private const int MaxPageSize = 100;
	private const int DefaultPageSize = 20;
	private const int DefaultPageNumber = 1;

	private int _pageSize = DefaultPageSize;
	private int _pageNumber = DefaultPageNumber;

	public int PageNumber
	{
		get => _pageNumber;
		set => _pageNumber = value < 1 ? DefaultPageNumber : value;
	}

	public int PageSize
	{
		get => _pageSize;
		set => _pageSize = value is <= 0 or > MaxPageSize ? DefaultPageSize : value;
	}

	public string? Search { get; set; }

	public int Skip => (PageNumber - 1) * PageSize;
	public int Take => PageSize;
}