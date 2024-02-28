using Learning_platform.Entities;

namespace Learning_platform.Models
{
    public class InitialRegisterModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public string UserType { get; set; }

    }

    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }


    public class AccountBasicDetailsModel 
    {
        public string Id { get; set; }   
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }

}
