using Learning_platform.DBContexts;
using Learning_platform.Entities;
using Microsoft.EntityFrameworkCore;

namespace Learning_platform.Services
{
    public class StudentRepository : IStudentRepository
    {

        private readonly TutoringPlatformContext _context;

        public StudentRepository(TutoringPlatformContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public async Task<IEnumerable<Student>> GetUsersAsync()
        {
            return await _context.Student.ToListAsync();
        }
    }
}
