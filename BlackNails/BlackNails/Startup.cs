using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlackNails.Startup))]
[assembly: log4net.Config.XmlConfigurator()]
namespace BlackNails
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
