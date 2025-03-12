using Dapper;
using Students.DataAccess.providers;

namespace Students.Application.Database;

public class DbInitializer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DbInitializer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var connection = await _dbConnectionFactory.CreateConnectionAsync();

        await connection.ExecuteAsync("""
        IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'STUDENTS')
    BEGIN
        CREATE TABLE students (
            id UNIQUEIDENTIFIER PRIMARY KEY,
            cprnumber NVARCHAR(255) NOT NULL,
            name NVARCHAR(255) NOT NULL,
            email NVARCHAR(255) NOT NULL,
            campus NVARCHAR(255) NOT NULL,
            gender NVARCHAR(10) NOT NULL CHECK (gender IN ('Male', 'Female', 'Other'))
        );
    END
""");
        
        await connection.ExecuteAsync("""
    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'subjects')
    BEGIN
        CREATE TABLE subjects (
            studentId UNIQUEIDENTIFIER REFERENCES students (Id),
            subjectname NVARCHAR(MAX) NOT NULL,
            grade NVARCHAR(MAX) NOT NULL
        );
    END
""");
    }
}
