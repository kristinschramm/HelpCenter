using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Hosting;
using Server;

namespace Server
{
    public class GmailRetrieval
    {
        // If modifying these scopes, delete your previously saved credentials 
        // at ~/.credentials/gmail-dotnet-quickstart.json
        static string[] Scopes = { GmailService.Scope.MailGoogleCom };
        private static readonly string UserName = "devseleniumhelpdesk@gmail.com";
        private static GMailService _service;

        public static void GetEmails()
        {
            var CRUD = new CRUD();
            if (!IsGMailServiceActive())
            {
                SetupGMailService();
            }
            
            var inboxlistRequest = _service.Service.Users.Messages.List(UserName);
            inboxlistRequest.LabelIds = "INBOX";
            inboxlistRequest.IncludeSpamTrash = false;
            //get our emails 

            while (true)
            {
                var emailListResponse = inboxlistRequest.Execute();

                if (emailListResponse != null && emailListResponse.Messages != null)
                {
                    //loop through each email and get what fields you want...
                    foreach (var email in emailListResponse.Messages)
                    {
                        var emailInfoRequest = _service.Service.Users.Messages.Get(UserName, email.Id);
                        var emailInfoResponse = emailInfoRequest.Execute();

                        string from = "";
                        string dateString = "";
                        string subject = "";
                        string decodedString = "";

                        if (emailInfoResponse != null)
                        {
                            
                            //loop through the headers to get from,date,subject, body  
                            foreach (var mParts in emailInfoResponse.Payload.Headers)
                            {
                                if (mParts.Name == "Date")
                                {
                                    dateString = mParts.Value;
                                }
                                else if (mParts.Name == "From") //Reply-To give email address without name
                                {
                                    from = mParts.Value;
                                    var emailAddressStart = from.IndexOf("<") + 1;
                                    var emailAddressEnd = from.IndexOf(">");
                                    from = from.Substring(emailAddressStart, emailAddressEnd - emailAddressStart);

                                }
                                else if (mParts.Name == "Subject")
                                {
                                    subject = mParts.Value;
                                }

                                if (dateString != "" && from != "")
                                {
                                    foreach (MessagePart p in emailInfoResponse.Payload.Parts)
                                    {
                                        if (p.MimeType == "text/plain")
                                        {
                                            byte[] data = FromBase64ForUrlString(p.Body.Data);
                                            decodedString = Encoding.UTF8.GetString(data);

                                        }
                                    }
                                }
                            }
                        }
                        DateTime dateTimeRecived;
                        DateTime.TryParse(dateString, out dateTimeRecived);
                        if(dateTimeRecived == DateTime.MinValue)
                        {
                            dateTimeRecived = DateTime.Now;
                        }
                        CRUD.CreateWorkOrder(from, dateTimeRecived, subject, decodedString);
                        DeleteEmail(email.Id);
                        
                    }
                }
            }

        }

        public static void DeleteEmail (string messageId)
        {
            if (!IsGMailServiceActive())
            {
                SetupGMailService();
            }
            _service.Service.Users.Messages.Trash(UserName, messageId).Execute();
        }

        

        public static byte[] FromBase64ForUrlString(string base64ForUrlInput)
        {
            int padChars = (base64ForUrlInput.Length % 4) == 0 ? 0 : (4 - (base64ForUrlInput.Length % 4));
            StringBuilder result = new StringBuilder(base64ForUrlInput, base64ForUrlInput.Length + padChars);
            result.Append(String.Empty.PadRight(padChars, '='));
            result.Replace('-', '+');
            result.Replace('_', '/');
            return Convert.FromBase64String(result.ToString());
        }

        private static bool IsGMailServiceActive()
        {
            return _service != null;
        }

        private static void SetupGMailService()
        {
            _service = new GMailService();
        }
    }
}


