namespace Students.Web.Clients.Models
{
    public class CreateStudentDto
    {
        public required string CprNumber { get; set; }

        public required string Name { get; set; }

        public required string Email { get; set; }

        public required GenderEnum Gender { get; set; }

        public required string Campus { get; set; }

        public required List<CompletedSubject> CompletedSubjects { get; set; } = new();
    }
}
