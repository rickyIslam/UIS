using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(UIS.Startup))]
namespace UIS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
