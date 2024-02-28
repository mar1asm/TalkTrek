using Learning_platform.Entities;

namespace Learning_platform.Services
{
    public interface IStudentRepository
    {
        Task<IEnumerable<Student>> GetUsersAsync();
    }
}
