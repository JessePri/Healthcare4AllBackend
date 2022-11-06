using HealthCare4All.Classes.Users;
using HealthCare4All.Models;

namespace HealthCare4All.Classes {
    public class UserAuthorizer {
        // NOTE: UserLogin will eventually be computed from a JWT token so re authentication doesn't occur.
        // Temporarily we will assume that this information is correct and came from a JST even though it didn't
        private Dictionary<UserLogin, User> CredentialsToUsers;
    }
}
