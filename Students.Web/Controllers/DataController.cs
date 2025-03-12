using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Students.Web.Clients;
using Students.Web.Clients.Models;

namespace Students.Web.Controllers;

public class DataController : Controller
{
  private readonly IStudentApiClient _studentApiClient;
  public DataController(IStudentApiClient studentApiClient)
  {
    _studentApiClient = studentApiClient;
  }

  public async Task<ActionResult<IEnumerable<StudentReadDto>>> Students([FromQuery] string? campus,
                                                                        [FromQuery] GenderEnum? gender,
                                                                        [FromQuery] string? sortField,
                                                                        [FromQuery] string? sortOrder,
                                                                        [FromQuery] int pageNumber,
                                                                        [FromQuery] int pageSize)
  {
    var students = await _studentApiClient.GetStudentsAsync(campus, gender, sortField, sortOrder, pageNumber, pageSize);
    return Ok(students);
  }

  [HttpPost]
  public async Task<IActionResult> CreateStudentAsync([FromForm] CreateStudentDto student)
  {
    if (ModelState.IsValid)
    {
      await _studentApiClient.CreateStudentAsync(student);
      return Ok();
    }
    return BadRequest(ModelState);
  }
}
