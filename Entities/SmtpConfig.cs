namespace Entities
{
    public class EmailConfig
    {
        public string SmtpSenderFti { get; set; } = string.Empty;
        public string SmtpNameFti { get; set; } = string.Empty;
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; }
        public string SmtpUsername { get; set; } = string.Empty;
        public string SmtpPassword { get; set; } = string.Empty;
    }
}