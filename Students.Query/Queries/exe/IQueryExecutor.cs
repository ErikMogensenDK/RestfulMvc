using System.Data;

namespace Students.Query.Queries.exe
{
  public interface IQueryExecutor
  {
    IEnumerable<T> Query<T>(IDbConnection cnn, string sql, object? param = null, IDbTransaction? transaction = null, bool buffered = true, int? commandTimeout = null, CommandType? commandType = null);
  }
}
