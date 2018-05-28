using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(FunTour.Startup))]
namespace FunTour
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
