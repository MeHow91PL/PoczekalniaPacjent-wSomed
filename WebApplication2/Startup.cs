using Microsoft.Owin;
using Owin;
using Poczekalniav1.DAL;
using Poczekalniav1.Infrastructure;

[assembly: OwinStartupAttribute(typeof(Poczekalniav1.Startup))]
namespace Poczekalniav1
{
    public class Startup
    {
        public void Configuration(IAppBuilder appBuilder)
        {
            appBuilder.MapSignalR();
            NasluchBazy nasluch = new NasluchBazy();
            OracleDbManager db = new OracleDbManager();
            nasluch.initApp(db.PobierzProszonychPacjentow());
        }
    }
}