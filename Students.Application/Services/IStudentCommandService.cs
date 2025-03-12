using Students.DataAccess.Requests;
using Students.Domain;

namespace Students.Application.Services;

public interface IStudentCommandService
{
	public Task<Student> CreateAsync(CreateStudentRequest student);
}