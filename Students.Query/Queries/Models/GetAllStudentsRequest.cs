using Students.Domain;

namespace Students.Query.Queries.Models;

public class GetAllStudentsRequest
{
	public required GenderEnum? Gender { get; init; }

	public required string? Campus { get; init; }

	public required string? SortField { get; init; }

	public required SortOrderEnum? SortOrder { get; init; }

	public int PageSize { get; init; } = 10;

    public int PageNumber { get; init; } = 1;
}
