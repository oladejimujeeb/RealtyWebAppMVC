using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json.Linq;
using RealtyWebApp.MailFolder.MailEntities;
using RealtyWebApp.MailFolder.EmailSettings;
using sib_api_v3_sdk.Api;
using sib_api_v3_sdk.Client;
using sib_api_v3_sdk.Model;
using Task = System.Threading.Tasks.Task;

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
             Configuration.Default.ApiKey.Add("api-key", _mailSetting.Password);

            var apiInstance = new TransactionalEmailsApi();
            string senderName = _mailSetting.DisplayName;
            string senderEmail = _mailSetting.Mail;
            SendSmtpEmailSender email = new SendSmtpEmailSender(senderName, senderEmail);
            string toEmail = message.Email;
            string toName = message.FullName;
            SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(toEmail, toName);
            List<SendSmtpEmailTo> to = new List<SendSmtpEmailTo>();
            to.Add(smtpEmailTo);
            string BccName = "CEO Realty Mulad";
            string BccEmail = "peerlessdomain@gmail.com";
            SendSmtpEmailBcc bccData = new SendSmtpEmailBcc(BccEmail, BccName);
            List<SendSmtpEmailBcc> bcc = new List<SendSmtpEmailBcc>();
            bcc.Add(bccData);
            string CcName = "CEO Realty Mulad";
            string CcEmail = "peerlessdomain@gmail.com";
            SendSmtpEmailCc ccData = new SendSmtpEmailCc(CcEmail, CcName);
            List<SendSmtpEmailCc> Cc = new List<SendSmtpEmailCc>();
            Cc.Add(ccData);
            string filePath = Directory.GetCurrentDirectory() + "\\MailFolder\\Templates\\WelcomeTemplate.html";
            StreamReader str = new StreamReader(filePath);
            string mailText = await str.ReadToEndAsync();
            str.Close();
            mailText = mailText.Replace("[username]", $"{message.FullName}").Replace("[RegId]",$"{message.Id}").Replace("[email]", $"{_mailSetting.Mail}");
            string htmlContent = mailText;//"<html><body><h1>This is my first transactional email {{params.parameter}}</h1></body></html>";
            string textContent = null;
            string Subject = "Confirmation of Registration - Realty Mulad";
            string ReplyToName = "Realty Mulad";
            string ReplyToEmail = "oladejimujib@gmail.com";
            SendSmtpEmailReplyTo replyTo = new SendSmtpEmailReplyTo(ReplyToEmail, ReplyToName);
            string attachmentUrl = null;
            string stringInBase64 = "aGVsbG8gdGhpcyBpcyB0ZXN0";
            byte[] content = System.Convert.FromBase64String(stringInBase64);
            string AttachmentName = "Realty.jpg";
            SendSmtpEmailAttachment attachmentContent = new SendSmtpEmailAttachment(attachmentUrl, content, AttachmentName);
            List<SendSmtpEmailAttachment> attachment = new List<SendSmtpEmailAttachment>();
            attachment.Add(attachmentContent);
            JObject Headers = new JObject();
            Headers.Add("Some-Custom-Name", "unique-id-1234");
            long? TemplateId = null;
            JObject Params = new JObject();
            Params.Add("parameter", "My param value");
            Params.Add("subject", "New Subject");
            List<string> Tags = new List<string>();
            Tags.Add("mytag");
            SendSmtpEmailTo1 smtpEmailTo1 = new SendSmtpEmailTo1(toEmail, toName);
            List<SendSmtpEmailTo1> to1 = new List<SendSmtpEmailTo1>();
            to1.Add(smtpEmailTo1);
            Dictionary<string, object> _parmas = new Dictionary<string, object>();
            _parmas.Add("params", Params);
            SendSmtpEmailReplyTo1 replyTo1 = new SendSmtpEmailReplyTo1(ReplyToEmail, ReplyToName);
            SendSmtpEmailMessageVersions messageVersion = new SendSmtpEmailMessageVersions(to1, _parmas, bcc, Cc, replyTo1, Subject);
            List<SendSmtpEmailMessageVersions> messageVersiopns = new List<SendSmtpEmailMessageVersions>();
            messageVersiopns.Add(messageVersion);
            try
            {
                var sendSmtpEmail = new SendSmtpEmail(email, to, bcc, Cc, htmlContent, textContent, Subject, replyTo, attachment, Headers, TemplateId, Params, messageVersiopns, Tags);
                CreateSmtpEmail result = await apiInstance.SendTransacEmailAsync(sendSmtpEmail);
                Configuration.Default.ApiKey.Clear();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                Console.WriteLine(e.Message);
            }
        }
    }
}