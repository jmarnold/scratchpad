using FubuMVC.Core;
using FubuMVC.Spark;
using FubuMVC.Validation;
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

            Actions
                .FindWith<JsonActionSource>();

            this.ApplyEndpointConventions();

            this.UseSpark();

            Policies
                .Add(new ValidationConvention(call => call.HasInput && call.InputType().Name.Contains("Input")));

            Routes
                .HomeIs<DashboardEndpoint>(e => e.Get(new DashboardRequest()));

            Views
                .TryToAttachWithDefaultConventions();

            HtmlConvention<ScratchpadHtmlConventions>();

            Output
                .ToJson
                .WhenCallMatches(call => call.HasOutput && call.OutputType().Name.Contains("Json"));
        }
    }
}