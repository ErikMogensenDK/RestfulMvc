using FluentValidation;
using Students.Query.Queries.Models;

namespace Students.Application.Validation;

public class GetAllStudentRequestValidator: AbstractValidator<GetAllStudentsRequest>
{
	public static List<string> ValidSortFields = new(){"name"};
	public static List<string> ValidSortOrders = new(){"ascending", "descending"};

	public GetAllStudentRequestValidator()
	{
		RuleFor(x => x.SortField)
			.Must(IsValidSortField);

		RuleFor(x => x.SortOrder)
			.Must(IsValidSortOrder);

		RuleFor(x => x.PageSize)
			.InclusiveBetween(1, 25);

		RuleFor(x => x.PageNumber)
			.GreaterThanOrEqualTo(1);
	}

    private bool IsValidSortField(string? arg)
    {
		if (arg is null)
			return true;

		return ValidSortFields.Contains(arg.ToLower());
    }

    private bool IsValidSortOrder(SortOrderEnum? sortOrder)
    {
		if(sortOrder is null) 
			return true;
		
		return ValidSortOrders.Contains(sortOrder.ToString()!.ToLower());
    }


}