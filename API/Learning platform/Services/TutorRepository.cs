using AutoMapper;
using Learning_platform.DBContexts;
using Learning_platform.Entities;
using Learning_platform.Models;
using Microsoft.EntityFrameworkCore;


namespace Learning_platform.Services
{
    public class TutorRepository
    {
        private readonly TutoringPlatformContext _context;
        private readonly IMapper _mapper;

        public TutorRepository(TutoringPlatformContext context, IMapper _mapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _mapper = _mapper ?? throw new ArgumentNullException(nameof(_mapper));
        }
        public async Task<IEnumerable<Tutor>> GetTutorsAsync()
        {
            return await _context.Tutor.ToListAsync();
        }

        public async Task<int> GetTutorCountAsync()
        {
            return await _context.Tutor.CountAsync();
        }

        public async Task<List<LanguageDto>> GetTutorLanguages(string tutorId)
        {
            return await _context.TutorLanguages
                .Where(tl => tl.Tutor.Id == tutorId)
                .Select(tl => new LanguageDto { Name = tl.Language.Name, Level = tl.Level })
                .ToListAsync();
        }


        public async Task<List<TeachingCategoryDto>> GetTutorTeachingCategories(string tutorId)
        {
            return await _context.TutorTeachingCategories
                .Where(tc => tc.Tutor.Id == tutorId)
                .Select (tc => new TeachingCategoryDto { Name = tc.TeachingCategory.Name })
                .ToListAsync();

        }


    }
}
