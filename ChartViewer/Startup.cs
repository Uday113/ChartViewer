using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ChartViewer.Startup))]
namespace ChartViewer
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
