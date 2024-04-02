using Learning_platform.DBContexts;
using Learning_platform.Entities;
using Microsoft.EntityFrameworkCore;

namespace Learning_platform.Services
{
    public class MessageRepository
    {
        private readonly TutoringPlatformContext _context;

        public MessageRepository(TutoringPlatformContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public async Task<MessageContentType?> GetContentTypeAsync(string name)
        {
            return await _context.MessageContentType.FirstOrDefaultAsync(m => m.Name == name);

        }

        public async Task<bool> CreateContentTypeAsync(string name)
        {

            try
            {
                _context.MessageContentType.Add(new MessageContentType(name));
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
