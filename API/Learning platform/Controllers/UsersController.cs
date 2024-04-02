using Microsoft.AspNetCore.Mvc;
using Learning_platform.Models;
using Learning_platform.Services;
using AutoMapper;
using System.Linq;
using Learning_platform.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Learning_platform.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : Controller
    {
        private readonly UserRepository _userRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public UsersController(UserRepository userRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository ?? 
                throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager ?? 
                throw new ArgumentNullException(nameof(userManager));
        }

        //GET: Users
        [HttpGet]
        public async Task<ActionResult <IEnumerable<UserDto>>> GetAllUsers()
        {
            var userEntities = await _userRepository.GetUsersAsync();

            /*var students = userEntities
                .Where(u => u.UserType.Name == UserTypeOptions.Student)
                .Select(_mapper.Map<StudentDto>);  // Map each student entity to StudentDto

            var tutors = userEntities
                .Where(u => u.UserType.Name == UserTypeOptions.Tutor)
                .Select(_mapper.Map<TutorDto>);    // Map each tutor entity to TutorDto

            var allUsers = students.Cast<UserDto>().Concat(tutors.Cast<UserDto>());
*/
            return Ok(userEntities);

        }

        //GET: User
        [HttpGet("email")]
        public async Task<ActionResult<UserDto>> GetUser(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return BadRequest(new
                {
                    message = "User email is required."
                });
            }

            var ApplicationUser = await _userManager.FindByEmailAsync(email);

            if (ApplicationUser == null)
            {
                return NotFound(new
                {
                    message = "Invalid user"
                });
            }

            var userEntity = await _userRepository.GetUserAsync(ApplicationUser.Id); //???????? bad

            /*var students = userEntities
                .Where(u => u.UserType.Name == UserTypeOptions.Student)
                .Select(_mapper.Map<StudentDto>);  // Map each student entity to StudentDto

            var tutors = userEntities
                .Where(u => u.UserType.Name == UserTypeOptions.Tutor)
                .Select(_mapper.Map<TutorDto>);    // Map each tutor entity to TutorDto

            var allUsers = students.Cast<UserDto>().Concat(tutors.Cast<UserDto>());
            */

            var userRole = await _userManager.GetRolesAsync(ApplicationUser);

            var user = _mapper.Map<UserDto>(userEntity);
            _mapper.Map(ApplicationUser, user);
            user.UserType = userRole.FirstOrDefault();

            return Ok(user);

        }

        [HttpGet("count")]
        public async Task<ActionResult<UserDto>> GetUserCount() { 
            var userCount = await _userRepository.GetUserCountAsync();
            return Ok(userCount);
        }


        [HttpGet("count/{role}")]
        public async Task<ActionResult<int>> GetUserCount(string role)
        {
            if (string.IsNullOrEmpty(role))
                return BadRequest("Role is required");
            var userEntitiesSpecificRole = new List<User>();
            var userEntities = await _userRepository.GetUsersAsync();
            foreach (var userEntity in userEntities)
            {
                var ApplicationUser = await _userManager.FindByIdAsync(userEntity.Id);
                var userRoles = await _userManager.GetRolesAsync(ApplicationUser);
                if (userRoles.First().Equals(role, StringComparison.OrdinalIgnoreCase))
                    userEntitiesSpecificRole.Add(userEntity);
            }
            var userCount = userEntitiesSpecificRole.Count();
            return Ok(userCount);
        }

    }
}
