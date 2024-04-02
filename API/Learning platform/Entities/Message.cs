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
        public virtual MessageContentType ContentType { get; set; }
        public virtual ApplicationUser Sender { get; set; }
        public virtual ApplicationUser Receiver { get; set; }
        public DateTime SentTime { get; set; } = DateTime.Now;
        public DateTime? ReadTime { get; set; }
        public bool IsRead { get; set; } = false;

    }
}
