using System;
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
            System.Net.Mail.MailMessage myMailMessage = new System.Net.Mail.MailMessage();
            myMailMessage.From = new System.Net.Mail.MailAddress(_mailSetting.Mail);
            myMailMessage.To.Add(message.Email);
            myMailMessage.Subject = "Confirmation of Registration - Realty Mulad";
            string filePath = Directory.GetCurrentDirectory() + "\\MailFolder\\Templates\\WelcomeTemplate.html";
            StreamReader str = new StreamReader(filePath);
            string mailText = await str.ReadToEndAsync();
            str.Close();
            mailText = mailText.Replace("[username]", $"{message.FullName}").Replace("[RegId]",$"{message.Id}");
            myMailMessage.Body =mailText;
            System.Net.Mail.SmtpClient smptServer = new System.Net.Mail.SmtpClient(_mailSetting.Host)
            {
                Port = _mailSetting.Port,
                Credentials = new System.Net.NetworkCredential(_mailSetting.Mail, _mailSetting.Password),
                EnableSsl = true
            };
            
            try
            {
                smptServer.Send(myMailMessage);
                      
            }
            catch (Exception ex)
            {
                string error = ex.Message;
                Console.WriteLine(error);
            }
            /*string filePath = Directory.GetCurrentDirectory() + "\\MailFolder\\Templates\\WelcomeTemplate.html";
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
            using (var smtp = new SmtpClient())
            {
                try
                {
                    /*smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    smtp.CheckCertificateRevocation = false;#1#
                    
                    await smtp.ConnectAsync(_mailSetting.Host, _mailSetting.Port,SecureSocketOptions.StartTls);
                    await smtp.AuthenticateAsync(_mailSetting.Mail, _mailSetting.Password);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                    
                }
                catch (Exception e)
                {
                    string error = e.Message;
                    Console.WriteLine(error);
                }
            }*/
        }
    }
}