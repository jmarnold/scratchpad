using System.Collections.Generic;
using FubuValidation;

namespace Scratchpad.Web.Endpoints
{
    public class JsonResponse
    {
        private readonly IList<ValidationError> _errors = new List<ValidationError>();

        public bool Success { get; set; }
        public string Message { get; set; }
        public IEnumerable<ValidationError> Errors { get { return _errors; } }

        public void RegisterErrors(IEnumerable<ValidationError> errors)
        {
            _errors.Fill(errors);
            Success = false;
        }

        public void RegisterError(ValidationError error)
        {
            RegisterErrors(new ValidationError[] {error});
        }
    }
}