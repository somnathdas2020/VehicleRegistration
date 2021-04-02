using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(VehicleRegistration.Startup))]
namespace VehicleRegistration
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
