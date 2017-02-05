using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BlackNails.Startup))]
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
