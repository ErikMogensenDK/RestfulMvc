namespace Students.Query.Responses;

public class PagedResponse<TResponse>
{
	public required List<TResponse> Items { get; init; } = new List<TResponse>();
	public required int PageSize { get; init; }
	public required int PageNumber { get; init; }
	public bool HasNextPage => Items.Count == 10;
}