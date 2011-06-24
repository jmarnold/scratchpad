using FubuMVC.Core;

namespace Scratchpad.Web.Configuration
{
    public class ScratchpadFubuRegistry : FubuRegistry
    {
        public ScratchpadFubuRegistry()
        {
            IncludeDiagnostics(true);

            Applies
                .ToThisAssembly();

            Views
                .TryToAttachWithDefaultConventions();
        }
    }

    // You want this reusable for integration testing
}