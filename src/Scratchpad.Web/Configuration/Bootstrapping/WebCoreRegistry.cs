using Scratchpad.Configuration;
using StructureMap.Configuration.DSL;

namespace Scratchpad.Web.Configuration.Bootstrapping
{
    public class WebCoreRegistry : Registry
    {
        public WebCoreRegistry()
        {
            Scan(x =>
                     {
                         x.TheCallingAssembly();
                         x.LookForRegistries();
                     });

            IncludeRegistry<CoreRegistry>();
        }
    }
}