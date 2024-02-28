using AutoMapper;
using Learning_platform.Models;
using Learning_platform.Services;
using Microsoft.AspNetCore.Mvc;

namespace Learning_platform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly IMapper _mapper;

        public StudentsController(IStudentRepository studentRepository, IMapper mapper)
        {
            _studentRepository = studentRepository ?? 
                throw new ArgumentNullException(nameof(studentRepository));
            _mapper = mapper ?? 
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<StudentDto>>> GetAllUsers() 
        {
            var studentEntities = await _studentRepository.GetUsersAsync();
            return Ok(_mapper.Map<IEnumerable<StudentDto>>(studentEntities));
        }

    }
}
