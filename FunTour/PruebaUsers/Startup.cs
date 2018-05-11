using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(PruebaUsers.Startup))]
namespace PruebaUsers
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
