



namespace Learning_platform.Models
{
    public class Student : UserDto
    {

        public Student() { }
        public Student(string firstName, string lastName, string email, string password, 
            UserType userType, DateTime registrationDate) 
            : base(firstName, lastName, email, password, userType, registrationDate)
        {
        }

        public Student(int id, string firstName, string lastName, string email, string password, 
            UserType userType, DateTime registration_date) 
            : base(id, firstName, lastName, email, password, userType, registration_date)
        {
        }
    }
}
