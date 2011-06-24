using System.Collections.Generic;
using System.Linq;

namespace Scratchpad
{
    public interface IUserRepository
    {
        IQueryable<User> Query();
    }

    public class UserRepository : IUserRepository
    {
        public IQueryable<User> Query()
        {
            return new List<User>
                       {
                           new User {FirstName = "Josh", LastName = "Arnold"},
                           new User {FirstName = "Alex", LastName = "Johannessen"},
                           new User {FirstName = "Jeremy", LastName = "Miller"}
                       }.AsQueryable();
        }
    }
}