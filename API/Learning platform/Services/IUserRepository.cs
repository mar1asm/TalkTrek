using Learning_platform.Entities;
using Learning_platform.Models;
using Microsoft.AspNetCore.Identity;

namespace Learning_platform.Services
{
    public interface IUserRepository
    {
        Task<IEnumerable <User>> GetUsersAsync();
        Task<bool> CompleteUserProfileAsync(AccountBasicDetailsModel model);

    }
}
