using Dapper;
using Students.DataAccess.exe;
using Students.DataAccess.providers;
using Students.Domain;
using Students.Domain.Repositories;

namespace Students.DataAccess.Repositories;

public class StudentRepository: IStudentRepository
{
    private readonly IDbConnectionFactory _connFactory;
    private readonly ICommandExecutor _exe;

    public StudentRepository(ICommandExecutor exe, IDbConnectionFactory connFactory)
    {
        _connFactory = connFactory;
        _exe = exe;
    }

    public async Task<bool> CreateAsync(Student student)
    {
		var (sql, parameters) = GenerateSqlAndParams(student);
        int affectedItems = 0;

        using (var conn = await _connFactory.CreateConnectionAsync())
        {
            affectedItems = _exe.Execute<int>(conn, sql, parameters);
        }

        if (affectedItems == 0)
            return false;
        return true;
    }

    private (string sql, object parameters) GenerateSqlAndParams(Student student)
    {
        var sql = @"insert into students (id, cprnumber, name, email, gender, campus) values (@Id, @CprNumber, @Name, @Email, @Gender, @Campus);";
		var parameters = new DynamicParameters();
        parameters.Add("Id", student.Id);
        parameters.Add("Name", student.Name);
        parameters.Add("CprNumber", student.CprNumber);
        parameters.Add("Email", student.Email);
        parameters.Add("Gender", student.Gender.ToString());
        parameters.Add("Campus", student.Campus);

        if (student.CompletedSubjects.Count < 1)
            return (sql, parameters);

        sql += $" insert into subjects (studentid, subjectname, grade) values";
        for (int i = 0; i < student.CompletedSubjects.Count; i++)
        {
            parameters.Add($"SubjectName{i}", student.CompletedSubjects[i].SubjectName);
            parameters.Add($"Grade{i}", student.CompletedSubjects[i].Grade);
            sql += $" (@Id, @SubjectName{i}, @Grade{i})";
            if (i != student.CompletedSubjects.Count-1)
                sql += ",";
        }
        sql += ";";
        return (sql, parameters);
    }
}
