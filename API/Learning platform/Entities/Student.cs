
namespace Learning_platform.Entities
{
    public class Student : User
    {
        public Student(string id, string firstName, string lastName, string country) 
            : base(id, firstName, lastName, country)
        {
        }
    }
}
