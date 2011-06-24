namespace Scratchpad.Web.Endpoints
{
    public class DashboardEndpoint
    {
        public string Get(DashboardRequestModel request)
        {
            return "Hello, World!";
        }
    }

    public class DashboardRequestModel { }
}