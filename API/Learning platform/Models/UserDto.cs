using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_platform.Models
{
    public class UserDto
    {
        public string Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserTypeDto UserType { get; set; }

        public DateTime RegistrationDate { get; set; }




    }

}
