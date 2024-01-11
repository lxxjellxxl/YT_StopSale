

using Entities;

namespace Plugins.Interfaces
{
    public interface IEmailServices
    {
        public void SendSmtpMail(SmtpConfig config, string subject, string bodyPath, string to, string? cc = null, string[]? attachments = null);

    }
}
