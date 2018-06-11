using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(HelpCenter.Startup))]
namespace HelpCenter
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
