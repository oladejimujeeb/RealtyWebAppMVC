using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using RealtyWebApp.MailFolder.MailEntities;
using RealtyWebApp.MailFolder.EmailSettings;

namespace RealtyWebApp.MailFolder.EmailService
{
    public class MailService:IMailService
    {
        private readonly MailSetting _mailSetting;

        public MailService(IOptions<MailSetting> mailSetting)
        {
            _mailSetting = mailSetting.Value;
        }
        public async Task WelcomeMail(WelcomeMessage message)
        {
            string filePath = Directory.GetCurrentDirectory() + "\\MailFolder\\Templates\\WelcomeTemplate.html";
            StreamReader str = new StreamReader(filePath);
            string mailText = await str.ReadToEndAsync();
            str.Close();
            mailText = mailText.Replace("[username]", $"{message.FullName}").Replace("[RegId]",$"{message.Id}");
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSetting.Mail);
            email.To.Add(MailboxAddress.Parse(message.Email));
            email.Subject = "Confirmation of Registration - Realty Mulad";
            
            var builder = new BodyBuilder();
            builder.HtmlBody = mailText;
            email.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSetting.Host, _mailSetting.Port, SecureSocketOptions.StartTls);
            await smtp.AuthenticateAsync(_mailSetting.Mail, _mailSetting.Password);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);
        }
    }
}