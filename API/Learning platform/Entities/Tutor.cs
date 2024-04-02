
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_platform.Entities
{
    public class Tutor : User
    {
        public string Description { get; set; }
        public float Price { get; set; }

        public string Education { get; set; }

        public float Rating { get; set; }

        public int Reviews { get; set; }

        public Tutor(string id, string firstName, string lastName, string country, string description, float price) 
            : base(id, firstName, lastName, country)
        {
            Description = description;
            Price = price;

        }
    }
}
