using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MimeKit;

namespace MailTesting1.Services
{

    public class MailService
    {
        ILogger<MailService> _logger { get; set; }
        MailSettings _mailSettings { get; set; }
        public MailService(IOptions<MailSettings> mailSettings, ILogger<MailService> logger)
        {
            _mailSettings = mailSettings.Value;
            _logger = logger;
        }
        public MimeMessage? _message {get; set; }
        public async Task<string> SendMail(MailContents mailContents)
        {
            MimeMessage message = new MimeMessage();
            message.From.Append<MailboxAddress>(new MailboxAddress(_mailSettings.UserName, _mailSettings.UserID));
            message.To.Append<MailboxAddress>(new MailboxAddress(mailContents.To, mailContents.ReceivedAddress));
            if (_mailSettings.ReplyTo != null)
            {
                foreach (KeyValuePair<string,string> email in _mailSettings.ReplyTo)
                {

                    _logger.LogInformation("\n-----\n31\nKeyValuePair<s,s> ReplyTo : { " + email.Key + " : " + email.Value + " }\n\n------\n");
                    message.ReplyTo.Add(new MailboxAddress(email.Key, email.Value));
                }
            }
            // _message = message;
            // foreach (MailboxAddress email in _message.ReplyTo)
            //     {
            //         var kvPair = new KeyValuePair<string,string>("a","b");
            //         _logger.LogWarning("\n-----\nKeyValuePair<s,s> ReplyTo : { " + email.Name + " : " + email.Address + " }\n\n------\n");
            //     }
            

            // Add method won't override 'MailBoxAddress' if existed the 'MailBoxAddress' had the same 'address' property before
            message.ReplyTo.Add(new MailboxAddress(_mailSettings.UserName, _mailSettings.UserID));

            if (mailContents.Cc != null)
            {
                message.Cc.AddRange(mailContents.Cc);
            }
            if (mailContents.Bcc != null)
            {
                message.Bcc.AddRange(mailContents.Bcc);
            }
            message.Subject = mailContents.Subject;

            BodyBuilder bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = mailContents.Body;

            message.Body = bodyBuilder.ToMessageBody();
            
            await Task<string>.CompletedTask;
            return "Done!";

            // using var smtpClient = new MailKit.Net.Smtp.SmtpClient();
            // try
            // {
            //     await smtpClient.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            //     await smtpClient.AuthenticateAsync(_mailSettings.UserID, _mailSettings.PassWord);
            //     await smtpClient.SendAsync(message);
            //     smtpClient.Disconnect(true);
            //     return "Send Mail success!";
            // }
            // catch (System.Exception e)
            // {
            //     Console.WriteLine(e.Message);
            //     return "Failure sending mail!\n" + e.Message;
            // }
        }
    }
    // userName, userID, psw 
    public class MailSettings
    {
        public string? UserName { get; set; }
        public string? UserID { get; set; }
        public string? PassWord { get; set; }
        public Dictionary<string, string>? ReplyTo { get; set; } = new Dictionary<string, string>();
    }

    // (MailContents) IMPORTANT properties: To, ReceivedAddress, Cc, Bcc, Subject, Body
    //
    // 'ReplyTo' property is opitons
    //
    public class MailContents
    {
        public string? To { get; set; }
        public string? ReceivedAddress { get; set; }
        public InternetAddressList? Cc { get; set; } = new InternetAddressList();
        public InternetAddressList? Bcc { get; set; } = new InternetAddressList();

        public string? Subject { get; set; }
        public string? Body { get; set; }

        // 'ReplyTo' property is opitons
        // public InternetAddressList? ReplyTo { get; set; } = new InternetAddressList();
    }
}