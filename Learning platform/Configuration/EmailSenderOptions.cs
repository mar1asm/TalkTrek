namespace Learning_platform.Configuration
{
    public class EmailSenderOptions
    {
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FromAddress { get; set; }
        public bool EnableSsl { get; set; }
    }
}
