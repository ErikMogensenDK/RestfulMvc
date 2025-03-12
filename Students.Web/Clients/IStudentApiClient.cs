using Students.Web.Clients.Models;

namespace Students.Web.Clients
{
    public interface IStudentApiClient
	{
		Task<StudentsResponse> GetStudentsAsync(string? campus, 
												GenderEnum? gender, 
												string? sortField, 
												string? sortOrder, 
												int pageNumber, 
												int pageSize);

		Task<bool> CreateStudentAsync(CreateStudentDto student);
	}
}
