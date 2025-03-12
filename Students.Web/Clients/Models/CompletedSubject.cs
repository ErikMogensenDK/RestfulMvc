namespace Students.Web.Clients.Models
{
    public class CompletedSubject
    {
        public required string SubjectName { get; set; }
        public required string Grade { get; set; }

        public static List<string> ValidGrades = new() { "12", "10", "7", "4", "02", "00", "-3" };
    }
}
