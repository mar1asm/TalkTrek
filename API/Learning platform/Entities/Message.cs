using Learning_platform.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Learning_platform.Entities
{
    public class Message
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        public string Content { get; set; }
        public ContentTypeDto ContentType { get; set; }
        public UserDto Sender { get; set; }
        public UserDto Receiver { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public bool IsRead { get; set; }

        public Message(string content, DateTime sentDate, DateTime? readDate, bool isRead=false)
        {
            Content = content;
            /*ContentType = contentType;
            Sender = sender;
            Receiver = receiver;*/
            SentDate = sentDate;
            ReadDate = readDate;
            IsRead = isRead;
        }
    }
}
