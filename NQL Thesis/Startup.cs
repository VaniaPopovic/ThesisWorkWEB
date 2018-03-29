using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(NQL_Thesis.Startup))]
namespace NQL_Thesis
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
