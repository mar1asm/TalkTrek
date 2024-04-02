namespace Learning_platform.Entities
{
    public class TutorTeachingCategories
    {
        public int Id { get; set; }
        public virtual Tutor Tutor { get; set; }
        public virtual TeachingCategory TeachingCategory { get; set; }
    }
}
