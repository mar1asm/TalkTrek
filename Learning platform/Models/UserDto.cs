using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_platform.Models
{
    public abstract class UserDto
    {
        public int Id { get; private set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserType UserType { get; set; }

        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }

        protected UserDto() { }

        protected UserDto(int id, string firstName, string lastName, string email, string password, 
            UserType userType, DateTime registration_date)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            UserType = userType;
            RegistrationDate = registration_date;
        }

        protected UserDto(string firstName, string lastName, string email, string password, 
            UserType userType, DateTime registrationDate)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Password = password;
            UserType = userType;
            RegistrationDate = registrationDate;
        }

        public string FullName
        {
            get
            {
                return LastName + ", " + FirstName;
            }
        }

    }

}
