using Learning_platform.Entities;

namespace Learning_platform.Services
{
    public interface ITutorRepository
    {
        Task<IEnumerable<Tutor>> GetUsersAsync();
    }
}
