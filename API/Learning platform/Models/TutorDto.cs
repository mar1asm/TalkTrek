


using Learning_platform.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Learning_platform.Models
{
    public class TutorDto : UserDto
    {
        public new string UserType { get; set; } = "Tutor";
        public string Description { get; set; }
        public float Price { get; set; }
        public string Education { get; set; }

        public float Rating { get; set; }

        public int Reviews { get; set; }

        public List<LanguageDto> Languages { get; set; } = new List<LanguageDto>();

        public List<TeachingCategoryDto> TeachingCategories { get; set; } = new List<TeachingCategoryDto>();


    }

}
