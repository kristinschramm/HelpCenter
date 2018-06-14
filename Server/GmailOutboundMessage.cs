using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net.Mail;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using Server;

namespace Server
{
    public class GmailOutboundMessage
    {
        private static HelpDeskDataContext _context;
        private static GMailService _service;
        public static void ListenForEmail()
        {

            if (_context == null) { ActivateDBContext(); }
            while (true)
            {
                var emailToSend = (
                    from emails in _context.EMails
                    where emails.Sent == false
                    select emails
                    ).FirstOrDefault();

                if(emailToSend != null)
                {
                    SendIt(emailToSend);
                    emailToSend.Sent = true;
                    _context.SubmitChanges();
                    //try
                    //{
                    //    SendIt(emailToSend);
                    //    emailToSend.Sent = true;
                    //}
                    //catch (Exception e) 
                    //{
                    //    Console.WriteLine(e.StackTrace);
                    //}
                }
            }
        }

        private static void ActivateDBContext()
        {
            _context = new HelpDeskDataContext();
        }

        public static void SendIt(EMail email)
        {
            var msg = new AE.Net.Mail.MailMessage
            {
                Subject = email.Subject,
                Body = "Fix it yourself",
                From = new MailAddress("devseleniumhelpdesk@gmail.com")
            };
            msg.To.Add(new MailAddress(email.ToEmailAddress));
            msg.ReplyTo.Add(msg.From); // Bounces without this!!
            var msgStr = new StringWriter();
            msg.Save(msgStr);
            // Context is a separate bit of code that provides OAuth context;
            // your construction of GmailService will be different from mine.
            if (_service == null)
            {
                _service = new GMailService();
            }
            _service.Service.Users.Messages.Send(new Message
            {
                Raw = Base64UrlEncode(msgStr.ToString())
            }, "devseleniumhelpdesk@gmail.com").Execute();
        }

        private static string Base64UrlEncode(string input)
        {
            var inputBytes = Encoding.UTF8.GetBytes(input);
            // Special "url-safe" base64 encode.
            return Convert.ToBase64String(inputBytes)
              .Replace('+', '-')
              .Replace('/', '_')
              .Replace("=", "");
        }

    }
}