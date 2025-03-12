using System.Data;

namespace Students.DataAccess.exe
{
    public interface ICommandExecutor
    {
        int Execute<T>(IDbConnection cnn, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null);
    }
}
