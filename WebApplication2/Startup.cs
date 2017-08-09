using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Poczekalniav1.Startup))]
namespace Poczekalniav1
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.MapSignalR();
        }
    }
}