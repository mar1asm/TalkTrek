namespace Learning_platform.Entities
{
    public class TutorLanguages
    {
        public int Id { get; set; }
        public virtual Tutor Tutor { get; set; }
        public virtual Language Language { get; set; }
        public int Level { get; set; }
    }
}
