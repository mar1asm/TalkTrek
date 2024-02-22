using Learning_platform.Entities;
using Learning_platform.Models;

namespace Learning_platform.Services
{
    public interface IUserRepository
    {
        Task<IEnumerable <User>> GetUsersAsync();
        Task<bool> CompleteUserProfile(AccountBasicDetailsModel model);
    }
}
