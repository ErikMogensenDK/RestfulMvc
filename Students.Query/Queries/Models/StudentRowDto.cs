namespace Students.Query.Queries.Models
{
	public class StudentDataRowDto
	{
		public required Guid id { get; set; }
		public required string cprnumber { get; set; }
		public required string name { get; set; }
		public required string email { get; set; }
		public required string campus { get; set; }
		public required string gender { get; set; }
	}
}