using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Threading;
using System.Xml;

namespace Learning_platform.Models
{
    public class MessageDto
    {

        public int Id { get; private set; }
        public string Content { get; set; }
        public ContentTypeDto ContentType { get; set; }
        public UserDto Sender { get; set; } 
        public UserDto Receiver{ get; set; }   
        public DateTime SentDate { get; set; }
        public DateTime? ReadDate { get; set; }
        public bool IsRead { get; set; } 

    }
}
