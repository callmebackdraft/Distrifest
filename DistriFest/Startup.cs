using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DistriFest.Startup))]
namespace DistriFest
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
