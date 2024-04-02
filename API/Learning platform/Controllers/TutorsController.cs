using AutoMapper;
using Learning_platform.Entities;
using Learning_platform.Models;
using Learning_platform.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Learning_platform.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TutorsController : Controller
    {
        private readonly TutorRepository _tutorRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        public TutorsController(TutorRepository tutorRepository, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _tutorRepository = tutorRepository ??
                throw new ArgumentNullException(nameof(_tutorRepository));
            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TutorDto>>> GetAllUsers()
        {
            var tutorEntities = await _tutorRepository.GetTutorsAsync();
            var tutors = new List<TutorDto>();

            foreach (var tutorEntity in tutorEntities)
            {
                var ApplicationUser = await _userManager.FindByIdAsync(tutorEntity.Id);

                if (ApplicationUser == null)
                {
                    return NotFound(new
                    {
                        message = "Invalid user"
                    });
                }
                var tutor = _mapper.Map<TutorDto>(tutorEntity);
                _mapper.Map(ApplicationUser, tutor);

                tutor.Languages = await _tutorRepository.GetTutorLanguages(tutorEntity.Id);

                tutor.TeachingCategories = await _tutorRepository.GetTutorTeachingCategories(tutorEntity.Id);

                tutors.Add(tutor);
            }

            return Ok(tutors);


        }
    }
}
