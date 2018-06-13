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

namespace HelpCenter
{
    public class GmailOutboundMessage
    {

        public void SendIt()
        {

            var msg = new AE.Net.Mail.MailMessage
            {
                Subject = "Your Subject",
                Body = "Hello, World, from Gmail API!",
                From = new MailAddress("devseleniumhelpdesk@gmail.com")
            };
            msg.To.Add(new MailAddress("yourbuddy@gmail.com"));
            msg.ReplyTo.Add(msg.From); // Bounces without this!!
            var msgStr = new StringWriter();
            msg.Save(msgStr);


            // Context is a separate bit of code that provides OAuth context;
            // your construction of GmailService will be different from mine.
            var gmail = new GmailService(new BaseClientService.Initializer());
            var result = gmail.Users.Messages.Send(new Message
            {
                Raw = Base64UrlEncode(msgStr.ToString())
            }, "me").Execute();
            Console.WriteLine("Message ID {0} sent.", result.Id);
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