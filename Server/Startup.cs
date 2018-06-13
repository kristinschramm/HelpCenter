using Microsoft.Owin;
using Owin;
using Server;

[assembly: OwinStartupAttribute(typeof(Server.Startup))]
namespace Server
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            // ConfigureAuth(app);
            GmailRetrieval.GetEmails();
        }
    }
}
