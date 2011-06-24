using FubuMVC.Core;
using FubuMVC.Spark;
using Scratchpad.Web.Endpoints;

namespace Scratchpad.Web.Configuration
{
    public class ScratchpadFubuRegistry : FubuRegistry
    {
        public ScratchpadFubuRegistry()
        {
            IncludeDiagnostics(true);

            Applies
                .ToThisAssembly();

            this.ApplyEndpointConventions();

            this.UseSpark();

            Routes
                .HomeIs<DashboardEndpoint>(e => e.Get(new DashboardRequestModel()));

            Views
                .TryToAttachWithDefaultConventions();
        }
    }
}