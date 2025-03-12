using Microsoft.AspNetCore.Mvc;
using Students.Application.Services;
using Students.DataAccess.Requests;
using Students.Query.Queries.Models;
using Students.Query.Responses;


namespace Students.Api.Controllers;

[ApiController]
public class StudentsController: ControllerBase
{
	private readonly IStudentCommandService _command;
	private readonly IStudentQueryService _query;

	public StudentsController(IStudentCommandService service, IStudentQueryService query)
	{
		_command = service;
		_query = query;
	}

	[HttpPost(ApiEndpoints.Students.Create)]
	public async Task<IActionResult> CreateStudent([FromBody] CreateStudentRequest request)
	{
		var student = await _command.CreateAsync(request);
		return CreatedAtAction(nameof(Get), new{ id = student.Id}, student);
	}

	[HttpGet(ApiEndpoints.Students.GetAll)]
	public async Task<ActionResult<StudentsResponse>> GetAllStudents([FromQuery] GetAllStudentsRequest request)
	{
		var students = await _query.GetAllAsync(request);
		return Ok(students);
	}

	[HttpGet(ApiEndpoints.Students.Get)]
	public async Task<IActionResult> Get([FromRoute] Guid id)
	{
		throw new NotImplementedException();
	}
}
