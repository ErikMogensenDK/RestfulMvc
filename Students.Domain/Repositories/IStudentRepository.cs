
namespace Students.Domain.Repositories
{
	public interface IStudentRepository
	{
		public Task<bool> CreateAsync(Student student);

		// public Task<Student?> GetByIdAsync(Guid id);

		// public Task<IEnumerable<Student>> GetAllAsync();

		// public Task<bool> UpdateAsync(Student student);

		// public Task<bool> DeleteByIdAsync(Guid id);

		// public Task<bool> ExistsByIdAsync(Guid id);
	}

}
