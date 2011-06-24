using System;
using FubuMVC.Core.Runtime;
using FubuMVC.Validation;
using FubuValidation;
using Scratchpad.Web.Endpoints;

namespace Scratchpad.Web.Configuration
{
    public class JsonValidationFailurePolicy : IValidationFailurePolicy
    {
        private readonly IFubuRequest _request;
        private readonly IPartialFactory _factory;

        public JsonValidationFailurePolicy(IFubuRequest request, IPartialFactory factory)
        {
            _request = request;
            _factory = factory;
        }

        public bool Matches(Type modelType)
        {
            return true;
        }

        public void Handle(Type modelType, Notification notification)
        {
            var jsonResponse = new JsonResponse();
            jsonResponse.RegisterErrors(notification.ToValidationErrors());

            _request.Set(jsonResponse);

            _factory
                .BuildPartial(jsonResponse.GetType())
                .InvokePartial();
        }
    }
}