using AutoMapper;
using Learning_platform.DBContexts;
using Learning_platform.Entities;
using Learning_platform.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Learning_platform.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly TutoringPlatformContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        public UserRepository(TutoringPlatformContext context, UserManager<ApplicationUser> userManager, IMapper mapper) 
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userManager = userManager ?? throw new ArgumentNullException( nameof(userManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<bool> CompleteUserProfileAsync(AccountBasicDetailsModel model)
        {
            // Retrieve the user based on the provided user ID
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user == null)
            {
                // Handle the case where the user is not found
                return false;
            }

            var userEntity = _mapper.Map<User>(model);

            _context.User.Add(userEntity);
            var saveResult = await _context.SaveChangesAsync();

            return saveResult > 0;
        }

       
        /*public async Task<User?> GetUserAsync(int userId)
{
   throw new NotImplementedException();
}
*/
        public async Task<IEnumerable<User>> GetUsersAsync()
       {

            return await _context.User.ToListAsync();
       }

        /*public async Task<IEnumerable<UserType>> GetUsersTypesAsync()
        {

            return await _context.UserType.ToListAsync();
        }*/
    }
}
