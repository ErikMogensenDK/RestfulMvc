using Dapper;
using System.Data;

namespace Students.Query.Queries.exe
{
	public class QueryExecutor : IQueryExecutor
	{
		public IEnumerable<T> Query<T>(IDbConnection cnn, string sql, object? param = null, IDbTransaction? transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null)
		{
			Console.WriteLine(sql);
			return cnn.Query<T>(sql, param, transaction, buffered, commandTimeout, commandType);	
		}
	}
}
