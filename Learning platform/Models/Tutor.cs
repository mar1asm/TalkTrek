


namespace Learning_platform.Models
{
    public class Tutor : UserDto
    {
        public Tutor() { }
        public Tutor(string firstName, string lastName, string email, string password, UserType userType,
            DateTime registrationDate, string description, float price)
            : base(firstName, lastName, email, password, userType, registrationDate)
        {
            Description = description;
            Price = price;
        }

        public Tutor(int id, string firstName, string lastName, string email, string password, UserType userType,
            DateTime registration_date, string description, float price)
            : base(id, firstName, lastName, email, password, userType, registration_date)
        {
            Description = description;
            Price = price;
        }

        public string Description { get; set; }
        public float Price { get; set; }


    }

}
