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

namespace HelpCenter
{
    public class GmailRetrieval
    {
        // If modifying these scopes, delete your previously saved credentials 
        // at ~/.credentials/gmail-dotnet-quickstart.json
        static string[] Scopes = { GmailService.Scope.GmailReadonly };
        static string ApplicationName = "Gmail API .NET Quickstart";

        public static void GmailStart()
        {
            UserCredential credential;
            using (var stream =
              new FileStream(HostingEnvironment.MapPath(@"/client_secret.json"), FileMode.Open, FileAccess.Read))
            {
                string credPath = Environment.GetFolderPath(
                Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/gmail-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                  GoogleClientSecrets.Load(stream).Secrets,
                  Scopes,
                  "user",
                  CancellationToken.None,
                  new FileDataStore(credPath, true)).Result;
            }

            // Create Gmail API service. 
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            var inboxlistRequest = service.Users.Messages.List("devseleniumhelpdesk@gmail.com");
            inboxlistRequest.LabelIds = "INBOX";
            inboxlistRequest.IncludeSpamTrash = false;
            //get our emails 
            var emailListResponse = inboxlistRequest.Execute();

            if (emailListResponse != null && emailListResponse.Messages != null)
            {
                //loop through each email and get what fields you want...
                foreach (var email in emailListResponse.Messages)
                {
                    var emailInfoRequest = service.Users.Messages.Get("devseleniumhelpdesk@gmail.com", email.Id);
                    var emailInfoResponse = emailInfoRequest.Execute();

                    if (emailInfoResponse != null)
                    {
                        String from = "";
                        String date = "";
                        String subject = "";

                        //loop through the headers to get from, date, subject, body
                        foreach (var mParts in emailInfoResponse.Payload.Headers)
                        {
                            if (mParts.Name == "Date")
                            {
                                date = mParts.Value;
                            }
                            else if (mParts.Name == "From")
                            {
                                from = mParts.Value;
                            }
                            else if (mParts.Name == "Subject")
                            {
                                subject = mParts.Value;
                            }

                            if (date != "" && from != "")
                            {
                                foreach (MessagePart p in emailInfoResponse.Payload.Parts)
                                {
                                    if (p.MimeType == "text/html")
                                    {
                                        byte[] data = FromBase64ForUrlString(p.Body.Data);
                                        string decodedString = Encoding.UTF8.GetString(data);
                                    }
                                }
                            }
                        }
                    }
                }
            }
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
    }
}


