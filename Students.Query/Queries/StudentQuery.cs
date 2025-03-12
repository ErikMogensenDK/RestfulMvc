using Dapper;
using Students.Domain;
using Students.Query.Queries.exe;
using Students.Query.Queries.providers;
using Students.Query.Queries.Models;
using Students.Query.Responses;

namespace Students.Query.Queries
{
	public class StudentQuery : IStudentQuery
	{
		private readonly IDbConnectionFactory _connFactory;
		private readonly IQueryExecutor _queryExe;
		private static string[] ValidSortFieldValues = ["name"];

		public StudentQuery(IDbConnectionFactory connFactory, IQueryExecutor queryExe)
		{
			_connFactory = connFactory;
			_queryExe = queryExe;
		}

		public async Task<StudentsResponse> GetAllAsync(GetAllStudentsRequest request)
		{
			IEnumerable<StudentDataRowDto> dbResponse;
			using (var conn = await _connFactory.CreateConnectionAsync())
			{
				var (sql, parameters) = GetSqlAndParams(request);
				dbResponse = _queryExe.Query<StudentDataRowDto>(conn, sql, parameters);
			}

			var response = new StudentsResponse
			{
				Items = new List<StudentResponse>(),
				PageNumber = request.PageNumber,
				PageSize = request.PageSize,
			};
			if (dbResponse == null)
				return response;

			foreach (var dbRow in dbResponse)
			{
				response.Items.Add(
					MapDbResponseToStudentResponse(dbRow)
				);
			}
			return response;
		}


		private (string, object) GetSqlAndParams(GetAllStudentsRequest request)
		{
			string sql = "";
			DynamicParameters parameters = new();
			(sql, parameters) = GetFilterSqlAndParams(request.Gender, request.Campus, sql, parameters);
			(sql, parameters) = AddOrderingToSqlAndParams(request, sql, parameters);
			(sql, parameters) = AddPagination(request, sql, parameters);
			return (sql, parameters);
		}


		private (string, DynamicParameters) GetFilterSqlAndParams(GenderEnum? gender, string? campus, string sql, DynamicParameters parameters)
		{
			sql += "select id, cprnumber, name, email, campus, gender from students ";
			if (gender == null && campus == null)
				return (sql, parameters);

			sql += " where ";
			bool filterAlreadyAdded = false;
			if (gender != null)
			{
				sql += "gender = @Gender ";
				parameters.Add("Gender", gender.ToString());
				filterAlreadyAdded = true;
			}
			if (campus != null)
			{
				if (filterAlreadyAdded)
					sql += "and ";
				sql += "campus = @Campus ";
				parameters.Add("Campus", campus);
			}

			return (sql, parameters);
		}

		private (string sql, DynamicParameters parameters) AddOrderingToSqlAndParams(GetAllStudentsRequest request, string sql, DynamicParameters parameters)
		{
			if (!ValidSortFieldValues.Contains(request.SortField))
			{
				return (sql, parameters);
			}
			sql += $"order by {request.SortField} ";
			sql += request.SortOrder == SortOrderEnum.Descending ? "desc" : "asc";

			return (sql, parameters);
		}

		private (string sql, DynamicParameters parameters) AddPagination(GetAllStudentsRequest request, string sql, DynamicParameters parameters)
		{
			int pageOffset = (request.PageNumber - 1) * request.PageSize;
			sql += " offset @PageOffset rows ";
			sql += "fetch next @PageSize rows only";
			parameters.Add("PageOffset", pageOffset);
			parameters.Add("PageSize", request.PageSize);
			return (sql, parameters);
		}

		private StudentResponse MapDbResponseToStudentResponse(StudentDataRowDto dbRow)
		{
			return new StudentResponse
			{
				Id = dbRow.id,
				CprNumber = dbRow.cprnumber,
				Name = dbRow.name,
				Email = dbRow.email,
				Gender = Enum.Parse<GenderEnum>(dbRow.gender, true),
				Campus = dbRow.campus
			};
		}
	}

}
