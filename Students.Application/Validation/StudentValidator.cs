using FluentValidation;
using Students.Domain;

namespace Students.Application.Validation;

public class StudentValidator : AbstractValidator<Student>
{
    private static readonly List<string> validCampuses = new(){
            "odense",
            "esbjerg",
            "sønderborg",
            "kolding",
            "vejle"
        };

    public StudentValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty();

        RuleFor(x => x.CprNumber)
            .Must(IsValidCpr)
            .NotEmpty();

        RuleFor(x => x.Name)
            .NotEmpty();

        RuleFor(x => x.Email)
            .NotEmpty()
            .Must(IsValidEmail);

        RuleFor(x => x.Campus)
            .NotEmpty()
            .Must(IsValidCampus);
    }

    private bool IsValidCpr(string arg)
    {
        return true;
    }

    private bool IsValidCampus(string campus)
    {
        if (validCampuses.Contains(campus.ToLower()))
            return true;
        return false;
    }

    private bool IsValidEmail(string arg)
    {
        return true;
    }
}
