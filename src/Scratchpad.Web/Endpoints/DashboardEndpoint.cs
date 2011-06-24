using System.Collections.Generic;

namespace Scratchpad.Web.Endpoints
{
    public class DashboardEndpoint
    {
        private readonly IUserRepository _userRepository;

        public DashboardEndpoint(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public DashboardViewModel Get(DashboardRequestModel request)
        {
            return new DashboardViewModel
                       {
                           Users = _userRepository.Query()
                       };
        }
    }

    public class DashboardRequestModel { }

    public class DashboardViewModel
    {
        public DashboardViewModel()
        {
            Users = new List<User>();
        }

        public IEnumerable<User> Users { get; set; }
    }
}