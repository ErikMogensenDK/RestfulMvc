using Students.Application.Services;
using Students.Domain;
using Students.Query.Queries.Models;
using Moq;
using Students.Query.Queries;
using Students.Query.Responses;
using FluentValidation;

namespace Students.ApplicationTests
{
    [TestClass]
    public sealed class StudentsQueryServiceTests
    {
        [TestMethod]
        public async Task TestStudentQueryServiceMasking()
        {
            GetAllStudentsRequest request = new(){
                Gender = GenderEnum.Male,
                Campus = "Odense",
                SortField = "Name",
                SortOrder = SortOrderEnum.Ascending,
                PageSize = 25,
                PageNumber = 1
            };

            var queryResult = new StudentsResponse()
            {
                Items = new(){new(){
                Id = Guid.NewGuid(),
                CprNumber = "123456-1234",
                Name = "Testnavn",
                Email = "Test@mail.com",
                Gender = GenderEnum.Male,
                Campus = "Odense"
                }
                },
                PageSize = 25,
                PageNumber = 1,
            };

            var expResult = new StudentsResponse()
            {
                Items = new(){new(){
                Id = Guid.NewGuid(),
                CprNumber = "123456-****",
                Name = "Testnavn",
                Email = "Test@mail.com",
                Gender = GenderEnum.Male,
                Campus = "Odense"
                }
                },
                PageSize = 25,
                PageNumber = 1,
            };

            var mockQuery = new Mock<IStudentQuery>();
            mockQuery.Setup(x=> x.GetAllAsync(It.IsAny<GetAllStudentsRequest>()))
                     .ReturnsAsync(queryResult);

            var mockValidator = new Mock<IValidator<GetAllStudentsRequest>>();

            var service = new StudentQueryService(mockQuery.Object, mockValidator.Object);
            var actResult = await service.GetAllAsync(request);
            Assert.AreEqual(expResult.Items[0].CprNumber, actResult.Items[0].CprNumber);;
        }
    }
}
