using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Poczekalniav1.Infrastructure
{
    public static class Paths
    {
        public static string UserConfigPath { get { return HttpContext.Current.Server.MapPath("~/Infrastructure/UserConfig.userconfig"); } }
        public static string WebConfigApp { get { return HttpContext.Current.Server.MapPath("~/Web.config"); } }
    }
}