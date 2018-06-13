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
    public class GMailService
    {
        static string[] Scopes = { GmailService.Scope.MailGoogleCom };
        static string ApplicationName = "Gmail API .NET Quickstart";
        
        public GmailService Service { get; set; }

        public GMailService()
        {
            UserCredential credential;
            using (var stream =
              new FileStream(HostingEnvironment.MapPath(@"/client_secret.json"), FileMode.Open, FileAccess.ReadWrite))
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
            Service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });

        }
    }
}