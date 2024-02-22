
namespace Learning_platform.Entities
{
    public class Tutor : User
    {
        public string Description { get; set; }
        public float Price { get; set; }
        public Tutor(string id, string firstName, string lastName, string description, float price) 
            : base(id, firstName, lastName)
        {
            Description = description;
            Price = price;

        }
    }
}
