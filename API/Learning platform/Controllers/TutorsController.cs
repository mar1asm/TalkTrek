using AutoMapper;
using Learning_platform.Entities;
using Learning_platform.Models;
using Learning_platform.Services;
using Microsoft.AspNetCore.Mvc;

namespace Learning_platform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TutorsController : Controller
    {
        private readonly ITutorRepository _tutorRepository;
        private readonly IMapper _mapper;
        public TutorsController(ITutorRepository tutorRepository, IMapper mapper)
        {
            _tutorRepository = tutorRepository ??
                throw new ArgumentNullException(nameof(_tutorRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TutorDto>>> GetAllUsers()
        {
            var tutorEntities = await _tutorRepository.GetUsersAsync();

            return Ok(_mapper.Map<IEnumerable<TutorDto>>( tutorEntities ));


        }
    }
}
