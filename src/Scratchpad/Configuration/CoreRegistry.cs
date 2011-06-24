using StructureMap.Configuration.DSL;

namespace Scratchpad.Configuration
{
    public class CoreRegistry : Registry
    {
        public CoreRegistry()
        {
            Scan(x =>
                     {
                         x.TheCallingAssembly();
                         x.LookForRegistries();
                         x.WithDefaultConventions();
                     });
        }
    }
}