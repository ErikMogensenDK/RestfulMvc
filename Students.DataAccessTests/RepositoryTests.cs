using Students.DataAccess.Repositories;
//using Students.DataAccess.Requests;
using Students.Domain;
using Moq;
using Dapper;
using Students.DataAccess.exe;
using Students.DataAccess.providers;
using System.Data;
using Students.DataAccess.Requests;

namespace Students.DataAccessTests
{
    [TestClass]
    public sealed class RepositoryTests 
    {
        [TestMethod]
        public async Task Repository_CreateWorksAsExpected()
        {
            //Arrange Mock of ConnectionFacotry
			var conn = new Mock<IDbConnection>().Object;
			var connFactory = new Mock<IDbConnectionFactory>();
			connFactory.SetReturnsDefault(Task.FromResult(conn));

            //Initialize exp SQL string
            string expSql = "insert into students (id, cprnumber, name, email, gender, campus) values (@Id, @CprNumber, @Name, @Email, @Gender, @Campus); insert into subjects (studentid, subjectname, grade) values (@Id, @SubjectName0, @Grade0), (@Id, @SubjectName1, @Grade1), (@Id, @SubjectName2, @Grade2);";

            //Arrange Command Executor
            var exe = new Mock<ICommandExecutor>();
            var repo = new StudentRepository(exe.Object, connFactory.Object);
			var actSql = "";
			object actParams = new();
			exe.Setup(
					e => e.Execute<int>(conn, It.IsAny<string>(), It.IsAny<object?>(), null, null, null))
                .Callback<IDbConnection, string, object?, IDbTransaction?, int?, CommandType?>(
                        (conn, sql, param, trans, timeout, type) => { actSql = sql; actParams = param; })
                .Returns(1);

            // Initialize expected Student
            var expStudent = new Student
            {
                Id = Guid.NewGuid(),
                CprNumber = "123",
                Name = "NameOne",
                Email = "MyEmail@gmail.com",
                Gender = GenderEnum.Male,
                Campus = "Odense",
                CompletedSubjects = new List<CompletedSubject>(){
                    new() {
                        SubjectName = "Physics 101",
                        Grade = "12"
                    },
                    new() {
                        SubjectName = "Natural Language philosphy 101",
                        Grade = "-3"
                    },
                    new() {
                        SubjectName = "Programming 201",
                        Grade = "10"
                    }
                }
            };

            //Act
            var numOfOperations = await repo.CreateAsync(expStudent);

            //Assert that sql string is as expected
            Assert.AreEqual(expSql, actSql);

            //Assert that Dynamic Parameters are as expected
            var parameters = (DynamicParameters)actParams;
            Assert.AreEqual(true, numOfOperations);
            Assert.IsFalse(parameters is null);
            Assert.AreEqual(expStudent.Id, parameters.Get<Guid>("Id"));
            Assert.AreEqual(expStudent.Name, parameters.Get<string>("Name"));
            Assert.AreEqual(expStudent.Campus, parameters.Get<string>("Campus"));
            Assert.AreEqual(expStudent.Gender.ToString(), parameters.Get<string>("Gender"));
            Assert.AreEqual(expStudent.Email, parameters.Get<string>("Email"));
            Assert.AreEqual(expStudent.CprNumber, parameters.Get<string>("CprNumber"));
            Assert.AreEqual(expStudent.CompletedSubjects[0].SubjectName, parameters.Get<string>("SubjectName0"));
            Assert.AreEqual(expStudent.CompletedSubjects[0].Grade, parameters.Get<string>("Grade0"));
            Assert.AreEqual(expStudent.CompletedSubjects[1].SubjectName, parameters.Get<string>("SubjectName1"));
            Assert.AreEqual(expStudent.CompletedSubjects[1].Grade, parameters.Get<string>("Grade1"));
            Assert.AreEqual(expStudent.CompletedSubjects[2].SubjectName, parameters.Get<string>("SubjectName2"));
            Assert.AreEqual(expStudent.CompletedSubjects[2].Grade, parameters.Get<string>("Grade2"));
        }
    }
}
