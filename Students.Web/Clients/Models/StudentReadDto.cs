namespace Students.Web.Clients.Models
{
    public class StudentReadDto
	{
		public Guid Id { get; init; }

		public string CprNumber { get; init; }

		public string Name { get; init; }

		public string Email { get; init; }

		public GenderEnum Gender { get; init; }

		public string Campus { get; init; }
	}
}
