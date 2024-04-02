using AutoMapper;
using Learning_platform.DBContexts;
using Learning_platform.Entities;
using Learning_platform.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Learning_platform.Services
{
    public class UserRepository 
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

        public async Task<bool> UpdateProfile(string Id, UserDto model)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                // Handle the case where the user is not found
                return false;
            }

            var userEntity = await _context.User.FindAsync(Id);

            if (userEntity == null)
            {
                userEntity = _mapper.Map<User>(model);
                userEntity.Id = Id;
                _context.User.Add(userEntity);

            } else
            {
                _mapper.Map(model, userEntity); // Update user entity with new data from model

                _context.User.Update(userEntity);
            }

            var saveResult = await _context.SaveChangesAsync();
            if (saveResult > 0)
            { 
                user.IsProfileComplete = true;
                await _userManager.UpdateAsync(user);
            }

            return saveResult > 0;
        }

       
        public async Task<IEnumerable<User>> GetUsersAsync()
        {

            return await _context.User.ToListAsync();
        }

        public async Task<User?> GetUserAsync(string id)
        {
            return await _context.User.FindAsync(id);

        }

        public async Task<int> GetUserCountAsync()
        {
            return await _context.User.CountAsync();
        }

        public async Task<bool> UpdateProfilePhoto(string Id, string photo)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                // Handle the case where the user is not found
                return false;
            }

            var userEntity = await _context.User.FindAsync(Id);

            userEntity.ProfilePhoto = photo;

            _context.User.Update(userEntity);
         

            var saveResult = await _context.SaveChangesAsync();

            return saveResult > 0;
        }

        public async Task<bool> DeleteProfilePhoto(string Id)
        {
            var user = await _userManager.FindByIdAsync(Id);
            if (user == null)
            {
                // Handle the case where the user is not found
                return false;
            }
            var userEntity = await _context.User.FindAsync(Id);

            userEntity.ProfilePhoto = null;

            _context.User.Update(userEntity);


            var saveResult = await _context.SaveChangesAsync();

            return saveResult > 0;
        }

        /*public async Task<IEnumerable<UserType>> GetUsersTypesAsync()
        {

            return await _context.UserType.ToListAsync();
        }*/
    }
}
