using FubuCore.Reflection;
using FubuMVC.Validation;
using FubuValidation;
using FubuValidation.Fields;
using StructureMap.Configuration.DSL;

namespace Scratchpad.Web.Configuration.Bootstrapping
{
    public class ValidationBootstrapRegistry : Registry
    {
        public ValidationBootstrapRegistry()
        {
            var validationRegistry = new ScratchpadValidationRegistry() as IValidationRegistration;
            var registry = new FieldRulesRegistry(validationRegistry.FieldSources(), new TypeDescriptorCache());
            validationRegistry.RegisterFieldRules(registry);

            For<IValidationSource>()
                .Add(new FieldRuleSource(registry));

            For<IValidationFailureHandler>()
                .Use<ValidationFailureHandler>();

            For<IValidationFailurePolicy>()
                .Add<JsonValidationFailurePolicy>();

            For<IFieldRulesRegistry>()
                .Use(registry);

            For<IValidator>().Use<Validator>();
        }
    }
}