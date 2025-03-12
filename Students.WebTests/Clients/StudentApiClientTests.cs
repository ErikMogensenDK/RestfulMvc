using Microsoft.AspNetCore.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Students.Web.Clients.Models;
using Students.Web.Clients.Values;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Students.Web.Clients.Tests
{
    [TestClass()]
	public class StudentApiClientTests
	{
		[TestMethod()]
		public async Task GetStudentsAsyncTest()
		{
			//Arrange jsonResponse and path of mocked HttpClient
			var expCampus = "Odense";
			var expGender = GenderEnum.Female;
			var expPath = $"/api/students?gender={expGender}&campus={expCampus}&pageNumber=1&pageSize=10";

			var jsonResponse = @"
			{ ""items"": 
				[
                    {
                              ""id"": ""8d39b2fb-7666-4e8e-b023-1d2e2fabae60"",
                              ""cprNumber"": ""cpr"",
                              ""name"": ""Name"",
                              ""email"": ""Email"",
                              ""gender"": 0,
                              ""campus"": ""Odense""
                    },
                    {
                              ""id"": ""26cd33cf-7688-468c-a01f-5a8bd074b997"",
                              ""cprNumber"": ""cpr2"",
                              ""name"": ""Name2"",
                              ""email"": ""mail@mail.com"",
                              ""gender"": 1,
                              ""campus"": ""vejen""
                    }
          ],
		""pageSize"": 10,
  		""pageNumber"": 1,
  		""hasNextPage"": false
		  }";
			var networkSetup = new NetworkSetup(HttpMethod.Get, expPath, jsonResponse);
			StudentApiAddress studentApiAddress = new("https://123.com");

			//Assertions of HttpMethod, Path and request content are done in the mocked httpMessageHandler
			var handler = new MockHttpHandler(networkSetup);
			var http = new HttpClient(handler);
			var client = new StudentApiClient(http, studentApiAddress);


			// Initialize expected object
			var expResponse = GetExpStudentResponse();

			//Act
			var response = await client.GetStudentsAsync(expCampus, expGender, null, null);

			//Assert
			var studentsResponse = response.Items.ToList();
			Assert.AreEqual(expResponse?.Items.Count, studentsResponse.Count());
			for(var i = 0; i < expResponse?.Items.Count;i++)
			{
				var exp = expResponse.Items[i];
				var act = studentsResponse[i];
				Assert.AreEqual(exp.Id , act.Id);
				Assert.AreEqual(exp.CprNumber, act.CprNumber);
				Assert.AreEqual(exp.Name, act.Name);
				Assert.AreEqual(exp.Email, act.Email);
				Assert.AreEqual(exp.Gender, act.Gender);
				Assert.AreEqual(exp.Campus, act.Campus);
			}
		}

        [TestMethod()]
		public async Task CreateStudentAsyncTest()
		{

			// Setup variables for Mocked httpClient

			// initialize dto for expected returned object
			CreateStudentDto dto = new(){
				CprNumber = "12345",
				Name = "TestNavn",
				Email = "TestEmail@mail.com",
				Gender = GenderEnum.Male,
				Campus = "Odense",
				CompletedSubjects = new(){
					new(){
						SubjectName = "SubjectNameOne",
						Grade = "12"
					},
					new(){
						SubjectName = "SubjectNameTwo",
						Grade = "-3"
					}
				}
			};
			// build requestMessage
			var jsonRequestMessageContent = JsonSerializer.Serialize(dto);
			HttpRequestMessage message = new();
			message.Content = new StringContent(jsonRequestMessageContent, Encoding.UTF8, "application/json");

			// Setup jsonResponse and path for HttpMessageHandler 
			var expPath = $"/api/students";
			var jsonResponseForSetup = @"
			{
          ""id"": ""7fa7416f-9cf7-40fe-a027-b36c6d7cb58d"",
          ""cprNumber"": ""12345"",
          ""name"": ""TestNavn"",
          ""email"": ""TestEmail@mail.com"",
          ""gender"": 0,
          ""campus"": ""Odense"",
		   ""completedSubjects"": [
    {
      ""subjectName"": ""SubjectNameOne"",
      ""grade"": ""12""
    },
    {
      ""subjectName"": ""SubjectNameTwo"",
      ""grade"": ""-3""
    }
  ]
}";
			var networkSetup = new NetworkSetup(HttpMethod.Post, expPath, jsonResponseForSetup, message);

			StudentApiAddress studentApiAddress = new("https://123.com");
			var handler = new MockHttpHandler(networkSetup);
			var http = new HttpClient(handler);
			var client = new StudentApiClient(http, studentApiAddress);

			//Act
			//Assertions of HttpMethod, Path and request content happen within the mocked httpMessageHandler
			var response = await client.CreateStudentAsync(dto);

			//Assert
			Assert.AreEqual(true, response);
		}

		private string GetExpectedResponseAsJson()
		{
			var GetExpResponseAsJson = @"{
				""items"": [
                    {
                              ""id"": ""8d39b2fb-7666-4e8e-b023-1d2e2fabae60"",
                              ""cprNumber"": ""cpr"",
                              ""name"": ""Name"",
                              ""email"": ""Email"",
                              ""gender"": 0,
                              ""campus"": ""Odense""
                    },
                    {
                              ""id"": ""26cd33cf-7688-468c-a01f-5a8bd074b997"",
                              ""cprNumber"": ""cpr2"",
                              ""name"": ""Name2"",
                              ""email"": ""mail@mail.com"",
                              ""gender"": 1,
                              ""campus"": ""vejen""
                    }
				],
  		""pageNumber"": 1,
		""pageSize"": 10,
  		""hasNextPage"": false
		}
				";
			return GetExpResponseAsJson;
		}

        private StudentsResponse GetExpStudentResponse()
        {
			var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
			var expResponse = JsonSerializer.Deserialize<StudentsResponse>(GetExpectedResponseAsJson(), options);
			return expResponse;
        }
	}


	internal class NetworkSetup
	{
		public readonly HttpMethod Method;
		public readonly string Path;
		public readonly string JsonResponse;
		public readonly HttpRequestMessage Message;
		public NetworkSetup(HttpMethod method, string path, string jsonResponse, HttpRequestMessage message = null)
		{
			Method = method;
			Path = path;
			JsonResponse = jsonResponse;
			Message = message;
		}
	}

	internal class MockHttpHandler : HttpMessageHandler
	{
		private readonly NetworkSetup _setup;
		public MockHttpHandler(NetworkSetup setup)
		{
			_setup = setup;
		}

		protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
		{
			Assert.IsTrue(_setup.Method == request.Method, $"test network setup unexpected, want: {_setup.Method} got {request.Method}");
			Assert.IsTrue(_setup.Path == request.RequestUri.PathAndQuery, $"test network setup unexpected, want: {_setup.Path} got {request.RequestUri.PathAndQuery}");
			if (request.Content != null)
			{
				var expContent = await _setup.Message.Content.ReadAsStringAsync();
				var actContent = await request.Content.ReadAsStringAsync();
				Assert.AreEqual(expContent, actContent);
			}
			var msg = new HttpResponseMessage();
			msg.Content = new StringContent(_setup.JsonResponse);
			return await Task.FromResult(msg);
		}
	}

}
