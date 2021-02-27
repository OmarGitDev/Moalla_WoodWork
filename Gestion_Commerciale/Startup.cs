using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Gestion_Commerciale.Startup))]
namespace Gestion_Commerciale
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
