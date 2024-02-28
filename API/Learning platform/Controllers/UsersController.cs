using Microsoft.AspNetCore.Mvc;
using Learning_platform.Models;
using Learning_platform.Services;
using AutoMapper;
using System.Linq;

namespace Learning_platform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository ?? 
                throw new ArgumentNullException(nameof(userRepository));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
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
            return Ok();

        }

    }
}
