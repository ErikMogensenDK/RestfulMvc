namespace Students.Domain
{
    public class Student
	{
		public required Guid Id { get; init; }

		public required string CprNumber { get; init; }

		public required string Name { get; init; }

		public required string Email { get; init; }

		public required GenderEnum Gender { get; init; }

		public required string Campus { get; init; }

		public required List<CompletedSubject> CompletedSubjects { get; init; } = new List<CompletedSubject>();
	}
}
