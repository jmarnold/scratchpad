using System.Web.Routing;
using Bottles;
using FubuMVC.Core;
using FubuMVC.StructureMap;
using Scratchpad.Web.Configuration.Bootstrapping;
using StructureMap;

namespace Scratchpad.Web.Configuration
{
    public class WebBootstrapper
    {
        public static void Bootstrap()
        {
            ObjectFactory
                .Initialize(x => x.AddRegistry<WebCoreRegistry>());

            FubuApplication
                .For<ScratchpadFubuRegistry>()
                .StructureMap(ObjectFactory.Container)
                .Bootstrap(RouteTable.Routes);

            PackageRegistry.AssertNoFailures();
        }
    }
}