using Learning_platform.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_platform.Entities
{
    public class User 
    {

        [Key]
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName {  get; set; }

        public string? ProfilePhoto { get; set; }

        public string? Country { get; set; }

        public User(string id, string firstName, string lastName, string? country)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Country = country;
        }
    }
}
