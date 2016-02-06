using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(AlwayEncryptedSample.Startup))]
namespace AlwayEncryptedSample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
