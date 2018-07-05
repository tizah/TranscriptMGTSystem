using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(TranscriptMGTSystem.Startup))]
namespace TranscriptMGTSystem
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
