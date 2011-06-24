using FubuValidation;
using FubuValidation.Fields;

namespace Scratchpad.Web.Configuration
{
    public class ScratchpadValidationRegistry : ValidationRegistry
    {
        public ScratchpadValidationRegistry()
        {
            FieldSource<AttributeFieldValidationSource>();

            ApplyRule<GreaterThanZeroRule>()
                .If(p => p.Name.ToLower().EndsWith("id"));
        }
    }
}