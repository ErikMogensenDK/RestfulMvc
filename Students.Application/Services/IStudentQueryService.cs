using Students.Query.Queries.Models;
using Students.Query.Responses;

namespace Students.Application.Services;

public interface IStudentQueryService
{
	Task<StudentsResponse> GetAllAsync(GetAllStudentsRequest request);
}
