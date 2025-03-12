using FluentValidation;
using Students.Domain.Repositories;
using Students.Domain;
using Students.DataAccess.Requests;

namespace Students.Application.Services;

public class StudentCommandService : IStudentCommandService
{
  private readonly IStudentRepository _studentRepository;
  private readonly IValidator<Student> _studentValidator;

  public StudentCommandService(IStudentRepository studentRepository, IValidator<Student> studentValidator)
  {
    _studentRepository = studentRepository;
    _studentValidator = studentValidator;
  }

  public async Task<Student> CreateAsync(CreateStudentRequest request)
  {
    var student = await MapToStudent(request);
    await _studentRepository.CreateAsync(student);
    return student;
  }

  private async Task<Student> MapToStudent(CreateStudentRequest request)
  {
    var student = new Student
    {
      Id = Guid.NewGuid(),
      CprNumber = request.CprNumber,
      Name = request.Name,
      Email = request.Email,
      Gender = request.Gender,
      Campus = request.Campus,
      CompletedSubjects = new List<CompletedSubject>()
    };

    await _studentValidator.ValidateAndThrowAsync(student);

    foreach (var subject in request.CompletedSubjects)
    {
      student.CompletedSubjects.Add(new CompletedSubject() { SubjectName = subject.SubjectName, Grade = subject.Grade });
    }
    return student;
  }

}