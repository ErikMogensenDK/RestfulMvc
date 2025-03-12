namespace Students.Web.Clients.Models
{
    public class StudentsResponse
    {
        public List<StudentReadDto> Items { get; set; } = new();
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
        public bool HasNextPage => Items.Count == 10;
    }
}
