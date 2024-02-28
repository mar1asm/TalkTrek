using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Learning_platform.Models
{
    public static class UserTypeOptions
    {
        public const string Tutor = "Tutor";
        public const string Student = "Student";
    }

    public class UserTypeDto
    {
        public int Id { get; set; }
        public String Name { get; set; } = String.Empty;

    }
}
