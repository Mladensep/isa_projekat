using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(IsaProjekat.WebApp.Startup))]
namespace IsaProjekat.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
