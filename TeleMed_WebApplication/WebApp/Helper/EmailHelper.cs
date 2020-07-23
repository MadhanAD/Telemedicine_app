using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SendGrid;
using System.Net.Mail;
using System.Net;
using System.Threading.Tasks;
using System.Configuration;
using Plivo;


namespace WebApp.Helper
{
    public class EmailHelper
    {
        #region Declarations

        public string toEmailAddress { get; set; }
        public string bccEmailAddress { get; set; }
        public string fromEmailAddress { get; set; }
        public string fromUserName { get; set; }
        public string subject { get; set; }
        public string message { get; set; }
        public string attachmentPath { get; set; }

        private string sendGridUserName = ConfigurationManager.AppSettings["SendGridUserName"].ToString();
        private string sendGridPassword = ConfigurationManager.AppSettings["SendGridPassword"].ToString();
        #endregion

        #region Constructors

        public EmailHelper()
        {
            fromEmailAddress = ConfigurationManager.AppSettings["SendGridFromEmailAddress"].ToString();
            fromUserName = ConfigurationManager.AppSettings["SendGridFromName"].ToString();
        }

        public EmailHelper(string toEmailAddress, string subject, string message) : this()
        {
            this.toEmailAddress = toEmailAddress;
            this.subject = subject;
            this.message = message;
        }

        public EmailHelper(string toEmailAddress, string subject, string message, string attachmentPath)
            : this(toEmailAddress, subject, message)
        {
            this.attachmentPath = attachmentPath;
        }

        #endregion

        #region Functions

        public void SendMessage()
        {
            //MailMessage m = new MailMessage();
            //SmtpClient sc = new SmtpClient();

            //m.From = new MailAddress(fromEmailAddress, (string.IsNullOrEmpty(fromUserName) ? fromEmailAddress : fromUserName));
            //m.To.Add(new MailAddress(toEmailAddress, "Display name To"));
            //m.Subject = subject;
            //m.IsBodyHtml = true;
            //m.Body = message;
            //sc.Host = "smtp.gmail.com";
            //sc.Port = 587;
            //sc.UseDefaultCredentials = true;
            ////sc.Credentials = new System.Net.NetworkCredential(sendGridUserName, sendGridPassword);
            //sc.EnableSsl = true; // runtime encrypt the SMTP communications using SSL
            //sc.Send(m);

            MailMessage mail = new MailMessage();
            SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");
            mail.From = new MailAddress(fromEmailAddress, (string.IsNullOrEmpty(fromUserName) ? fromEmailAddress : fromUserName));
            mail.To.Add(new MailAddress(toEmailAddress, "Display name To"));
            mail.Subject = subject;
            mail.IsBodyHtml = true;
            mail.Body = message;
            SmtpServer.Port = 587;
            SmtpServer.UseDefaultCredentials = true;
            SmtpServer.Credentials = new System.Net.NetworkCredential(sendGridUserName, sendGridPassword);            
            SmtpServer.EnableSsl = true;
            //Felix
            //SmtpServer.Send(mail);
            //-------
            //// SendGridMessage myMessage = new SendGridMessage();
            //// myMessage.AddTo(toEmailAddress);
            //// myMessage.From = new MailAddress(fromEmailAddress, (string.IsNullOrEmpty(fromUserName) ? fromEmailAddress : fromUserName));

            //// if (!string.IsNullOrEmpty(bccEmailAddress))
            //// {
            ////     myMessage.AddBcc(bccEmailAddress);
            //// }

            //// myMessage.Subject = subject;
            //// myMessage.Html = message;

            //// if (!string.IsNullOrEmpty(attachmentPath))
            //// {
            ////     myMessage.AddAttachment(attachmentPath);
            //// }

            //// var credentials = new NetworkCredential(sendGridUserName, sendGridPassword);

            //// Create an Web transport for sending email.

            ////var transportWeb = new Web(credentials);

            //// Send the email, which returns an awaitable task.
            ////var oRet = transportWeb.DeliverAsync(myMessage);
            //return "ok";
        }

        public void SendSMS()
        {
            string Tonumber = "+919003820238";
            string smessage = "Telemedicine SMS Testing";
            var api = new PlivoApi("MANTQWNJFINDM1MJA5Y2", "ZWNjM2M3ZWY0MDIzM2FjZjYyZGZmZDg4YmY5NzNi");
            //API SMS
            var response = api.Message.Create(
                src: "95656",
                dst: new List<String> { Tonumber.Replace(" ", "") },
                text: smessage
            );
        }

        #endregion
    }
}