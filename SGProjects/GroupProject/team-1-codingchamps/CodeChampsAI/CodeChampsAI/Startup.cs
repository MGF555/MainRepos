using Microsoft.Owin;
using Owin;

//[assembly: OwinStartupAttribute(typeof(CodeChampsAI.Startup))]
namespace CodeChampsAI
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}