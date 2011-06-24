using FubuMVC.Core;
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

            Routes
                .HomeIs<DashboardEndpoint>(e => e.Get(new DashboardRequestModel()));

            Views
                .TryToAttachWithDefaultConventions();
        }
    }
}