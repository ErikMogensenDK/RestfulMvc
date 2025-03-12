using Students.Domain;
using Students.Query.Queries.Models;
using Students.Query.Responses;

namespace Students.Query.Queries
{
	public interface IStudentQuery
	{
		Task<StudentsResponse> GetAllAsync(GetAllStudentsRequest request);
	}
}
