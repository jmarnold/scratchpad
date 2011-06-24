using FubuValidation;

namespace Scratchpad.Web.Endpoints.Users
{
    public class CreateEndpoint
    {
        public CreateUserViewModel Get(CreateUserRequest request)
        {
            return new CreateUserViewModel();
        }

        public JsonResponse Post(CreateUserInputModel input)
        {
            return new JsonResponse { Success = true};
        }
    }

    public class CreateUserRequest { }

    public class CreateUserInputModel
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }

    // Inherit the basics for html conventions but leave room for additional fields that won't get posted
    public class CreateUserViewModel : CreateUserInputModel
    {
    }
}