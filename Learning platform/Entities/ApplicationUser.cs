using Microsoft.AspNetCore.Identity;

namespace Learning_platform.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime RegistrationDate { get; set; } = DateTime.UtcNow;
        public ApplicationUser(string userName,string email) : base(userName)
        {
            Email = email;
            UserName = email;
        }
    }
}
