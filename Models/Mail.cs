using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;



namespace Models
{
    public class Mail
    {
        public SmtpClient Client { get; private set; }
        public string From { get; private set; }
        public string To { get; private set; }
        public string Subject { get; private set; }
        public string Message { get; private set; }
        private MailMessage BuiltMessage;

        public Mail(string _to, string _subject, string _message)
        {
            Client = new SmtpClient("mail.notacorrect.nl", 8889);
            Client.Credentials = new NetworkCredential("Distrifest@notacorrect.nl", "SystemMailer2018!");
            From = "Distrifest@notacorrect.nl";
            To = _to;
            Subject = _subject;
            Message = _message;
            GenerateMailMessage();
        }

        private void GenerateMailMessage()
        {
            BuiltMessage = new MailMessage(new MailAddress(From), new MailAddress(To));
            BuiltMessage.IsBodyHtml = true;
            BuiltMessage.Subject = Subject;
            BuiltMessage.Body = Message;
        }

        public void AddAttachment(Attachment _attachment)
        {
            BuiltMessage.Attachments.Add(_attachment);
        }

        public void SendMail()
        {
            Client.Send(BuiltMessage);
        }
    }
}
