using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ProcessManager.Startup))]
namespace ProcessManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
