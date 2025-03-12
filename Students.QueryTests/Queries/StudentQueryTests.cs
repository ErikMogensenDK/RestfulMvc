using Dapper;
using Moq;
using Students.Domain;
using Students.Query.Queries.exe;
using Students.Query.Queries.Models;
using Students.Query.Queries.providers;
using System.Data;

namespace Students.Query.Queries.Tests
{
	[TestClass()]
	public class StudentQueryTests
	{
		[TestMethod()]
		public async Task GetAllTest()
		{
			//Arrange mocking of ConnFacotry
			var conn = new Mock<IDbConnection>().Object;
			var connFactory = new Mock<IDbConnectionFactory>();
			connFactory.SetReturnsDefault(Task.FromResult(conn));

			//Arrange hard-coded response to be passed through queryExecutor 
			var expDataResponse = new List<StudentDataRowDto>{
				new StudentDataRowDto
				{
					id  = Guid.NewGuid(),
					cprnumber  = "123456-1234",
					name  = "name1",
					email  = "email1",
					campus  = "campus1",
					gender  = "Female"
				},
				new StudentDataRowDto
				{
					id  = Guid.NewGuid(),
					cprnumber  = "654321-4321",
					name  = "name2",
					email  = "email2",
					campus  = "campus2",
					gender  = "Male"
				}
			};

			// Arrange query Executor
			object actParams = new();
			var actSql = "";
			var queryExe = new Mock<IQueryExecutor>();
			queryExe.Setup(
					q => q.Query<StudentDataRowDto>(conn, It.IsAny<string>(), It.IsAny<object?>(), null, true, null, null))
				.Callback<IDbConnection, string,object?,IDbTransaction?,bool,int?,CommandType?>(
						(conn,sql,param,trans,buf,timeout,type) => {actParams = param!; actSql = sql;})
				.Returns(expDataResponse);

			//Arrange request
			var campus = "odense";
			var gender = GenderEnum.Male;
			GetAllStudentsRequest request = new(){Gender = gender, Campus = campus, SortField = "name", SortOrder = SortOrderEnum.Ascending};

			var expSql = $"select id, cprnumber, name, email, campus, gender from students  where gender = @Gender and campus = @Campus order by name asc offset @PageOffset rows fetch next @PageSize rows only";

			//Act
			var query = new StudentQuery(connFactory.Object, queryExe.Object);
			var actualDataResponse = (await query.GetAllAsync(request)).Items.ToList();

			//Assert that generatedSQL string and params are as expected
			Assert.AreEqual(expSql, actSql);
			Assert.IsNotNull(actParams);
			var actParam = (DynamicParameters)actParams;
			Assert.AreEqual(campus, actParam.Get<string>("Campus"));
			Assert.AreEqual(gender.ToString(), actParam.Get<string>("Gender"));
			Assert.AreEqual(10, actParam.Get<int>("PageSize"));
			Assert.AreEqual(0, actParam.Get<int>("PageOffset"));

			//Assert that returned objects are as expected
			Assert.AreEqual(expDataResponse.Count, actualDataResponse.Count);
			for(var i = 0; i < actualDataResponse.Count;i++)
			{
				var exp = expDataResponse[i];
				var act = actualDataResponse[i];
				Assert.AreEqual(exp.id, act.Id);
				Assert.AreEqual(exp.cprnumber, act.CprNumber);
				Assert.AreEqual(exp.name, act.Name);
				Assert.AreEqual(exp.email, act.Email);
				Assert.AreEqual(exp.campus, act.Campus);
				Assert.AreEqual(exp.gender, act.Gender.ToString());
			}

		}
	}
}
