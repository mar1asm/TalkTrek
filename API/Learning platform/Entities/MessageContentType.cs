namespace Learning_platform.Entities
{
    public class MessageContentType
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public MessageContentType(string Name)
        {
            this.Id = Guid.NewGuid().ToString(); // Set Id to a random GUID
            this.Name= Name;
        }
    }
}
