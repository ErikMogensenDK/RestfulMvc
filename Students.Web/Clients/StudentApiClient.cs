using Students.Web.Clients.Models;
using Students.Web.Clients.Values;
using System.Text;
using System.Text.Json;

namespace Students.Web.Clients
{
    public class StudentApiClient : IStudentApiClient
	{
		private readonly HttpClient _client;
		private readonly StudentApiAddress _baseUrl;

		public StudentApiClient(HttpClient client, StudentApiAddress baseUrl)
		{
			_client = client;
			_baseUrl = baseUrl;
		}

		public async Task<StudentsResponse> GetStudentsAsync(string? campus, 
															 GenderEnum? gender, 
															 string? sortField, 
															 string? sortOrder, 
															 int pageNumber = 1, 
															 int pageSize = 10) 
		{
			//Generate path
			string path = $"{_baseUrl.Value}api/students?";
			if (gender != null)
				path += $"gender={gender}&";
			if (!string.IsNullOrEmpty(campus))
				path += $"campus={campus}";
			if (!string.IsNullOrEmpty(sortField))
				path += $"&sortField={sortField}&";
			if (!string.IsNullOrEmpty(sortOrder))
				path += $"&sortOrder={sortOrder}";

			path += $"&pageNumber={pageNumber}";
			path += $"&pageSize={pageSize}";

			// generate and send message
			HttpRequestMessage message = new(HttpMethod.Get, path);
			var response = await _client.SendAsync(message);
			if (!response.IsSuccessStatusCode)
				return new StudentsResponse();

			// Handle/return response
			var jsonResponse = await response.Content.ReadAsStringAsync();
			var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			var students = JsonSerializer.Deserialize<StudentsResponse>(jsonResponse, options);
			if (students?.Items is null)
				return new StudentsResponse();
			return students;
		}

		public async Task<bool> CreateStudentAsync(CreateStudentDto student)
		{
			string path = $"{_baseUrl.Value}api/students";

			HttpRequestMessage message = new(HttpMethod.Post, path);
			var jsonContent = JsonSerializer.Serialize(student);
			message.Content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

			var response = await _client.SendAsync(message);
			if (!response.IsSuccessStatusCode)
				throw new Exception(response.StatusCode.ToString());
			return true;
		}

    }
}
