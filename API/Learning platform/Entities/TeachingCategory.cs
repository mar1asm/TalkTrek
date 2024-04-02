namespace Learning_platform.Entities
{
    public class TeachingCategory
    {
        public TeachingCategory(string name)
        {
            Name = name;
        }

        public int Id { get; set; }
        public string Name { get; set; }
    }
}
