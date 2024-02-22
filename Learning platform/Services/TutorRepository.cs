using Learning_platform.DBContexts;
using Learning_platform.Entities;
using Microsoft.EntityFrameworkCore;


namespace Learning_platform.Services
{
    public class TutorRepository: ITutorRepository
    {
        private readonly TutoringPlatformContext _context;

        public TutorRepository(TutoringPlatformContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<Tutor>> GetUsersAsync()
        {
            return await _context.Tutor.ToListAsync();
        }
    }
}
