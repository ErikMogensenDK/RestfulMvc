using Dapper;
using System.Data;

namespace Students.DataAccess.exe
{
    public class CommandExecutor : ICommandExecutor
    {
        public int Execute<T>(IDbConnection cnn, string sql, object? param = null, IDbTransaction? transaction = null, int? commandTimeout = null, CommandType? commandType = null)
        {
            return cnn.Execute(sql, param, transaction, commandTimeout, commandType);
        }
    }
}
