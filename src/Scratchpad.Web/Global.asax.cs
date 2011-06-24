using System;
using Scratchpad.Web.Configuration;

namespace Scratchpad.Web
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            WebBootstrapper.Bootstrap();
        }
    }
}