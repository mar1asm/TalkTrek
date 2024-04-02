using Learning_platform.DBContexts;
using Learning_platform.Entities;
using Microsoft.EntityFrameworkCore;

namespace Learning_platform.Services
{
    public class LanguageRepository
    {
        private readonly TutoringPlatformContext _context;

        public LanguageRepository(TutoringPlatformContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<TeachingCategory?> GetTeachingCategoryAsync(string name)
        {
            return await _context.TeachingCategories.FirstOrDefaultAsync(m => m.Name == name);

        }

        public async Task<bool> CreateTeachingCategoryAsync(string name)
        {

            try
            {
                _context.TeachingCategories.Add(new TeachingCategory(name));
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        
    }
}
