using Microsoft.Owin;
using Owin;
using Server;
using System.Threading;

[assembly: OwinStartupAttribute(typeof(Server.Startup))]
namespace Server
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // ConfigureAuth(app);
            new Thread(new ThreadStart(GmailOutboundMessage.ListenForEmail)).Start();
            GmailRetrieval.GetEmails();
        }
    }
}
