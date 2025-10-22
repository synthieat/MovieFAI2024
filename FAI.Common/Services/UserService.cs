using FAI.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FAI.Common.Services
{
    public class UserService : IUserService
    {
        /* Mockup für Test User */
        private readonly List<User> users =
        [
            new User{
                Id = Guid.NewGuid(),
                FirstName = "Test",
                LastName = "User",
                Username = "Test",
                Passwort =  new NetworkCredential("Test", "12345").SecurePassword
            }
        ];


        public async Task<User> Authenticate(string username, string password, CancellationToken cancellationToken)
        {
            var user = users.SingleOrDefault(w => string.Compare(w.Username, username, true) == 0 &&
                                                  new NetworkCredential(w.Username, w.Passwort).Password == password);

            if (user == null)
            {
                return user;
            }

            // return await Task.CompletedTask(); => bei synchronen Tasks ohne Rückgabewert
            return await Task.FromResult(user.UserWithoutPassword);
        }


    }
}
