using FluentValidation;
using Students.Query.Queries;
using Students.Query.Queries.Models;
using Students.Query.Responses;

namespace Students.Application.Services;

public class StudentQueryService : IStudentQueryService
{
    private readonly IStudentQuery _query;
    private readonly IValidator<GetAllStudentsRequest> _validator;

    public StudentQueryService(IStudentQuery query, IValidator<GetAllStudentsRequest> validator)
    {
        _query = query;
        _validator = validator;
    }
    public async Task<StudentsResponse> GetAllAsync(GetAllStudentsRequest request)
    {
        await _validator.ValidateAndThrowAsync(request);
        var dbResult = await _query.GetAllAsync(request);
        return MaskCprOfResult(dbResult);
    }

    private StudentsResponse MaskCprOfResult(StudentsResponse dbResult)
    {
        var maskedResult = new StudentsResponse()
        {
            Items = new(),
            PageSize = dbResult.PageSize,
            PageNumber = dbResult.PageNumber
        };

        foreach(var result in dbResult.Items)
        {
            maskedResult.Items.Add(CopyItemWithMaskedCpr(result));
        }
        return maskedResult;
    }

    private StudentResponse CopyItemWithMaskedCpr(StudentResponse result)
    {
        var response = new StudentResponse(){
            Id = result.Id,
            CprNumber = MaskCpr(result.CprNumber),
            Name = result.Name,
            Email = result.Email,
            Gender = result.Gender,
            Campus = result.Campus
        };
        return response;
    }

    private string MaskCpr(string cprNumber)
    {
        if (cprNumber.Length > 5)
        {
            var maskedCpr = cprNumber[0..6];
            maskedCpr += "-****";
            return maskedCpr;
        }
        return cprNumber;
    }
}
