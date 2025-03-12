using System.Data;

namespace Students.DataAccess.providers
{

    public interface IDbConnectionFactory
    {
        Task<IDbConnection> CreateConnectionAsync(CancellationToken token = default);
    }
}
