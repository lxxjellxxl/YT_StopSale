using Plugins.Interfaces;
using System.Net.Mail;
using System.Net;
using Entities;
using Microsoft.Extensions.Options;

namespace Plugins
{
    public class EmailServices : IEmailServices
    {

        public void SendSmtpMail(SmtpConfig config, string subject, string bodyPath, string to, string? cc = null, string[]? attachments = null)

        {
            MailMessage mailMessage = new MailMessage
            {
                From = new MailAddress(config.SmtpSender, config.SmtpName),
                Subject = subject,
                Body = File.ReadAllText(bodyPath),
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(to));
            if (cc != null)
            {
                mailMessage.CC.Add(cc);
            }


            if (attachments != null && attachments.Length > 0)
            {
                foreach (var attachmentPath in attachments)
                {
                    // Attach each file in the list
                    mailMessage.Attachments.Add(new Attachment(attachmentPath));
                }
            }

            using (SmtpClient smtpClient = new SmtpClient
            {
                Host = config.SmtpServer,
                Port = config.SmtpPort,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(config.SmtpUsername, config.SmtpPassword),
                // EnableSsl = true
            })
            {
                smtpClient.Send(mailMessage);
            }
        }
    }
}

