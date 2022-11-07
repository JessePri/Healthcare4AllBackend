using HealthCare4All.Data;
using HealthCare4All.Models;

namespace HealthCare4All.Classes.Users {
    public abstract class User {
        
        protected User(string newUserName, Healthcare4AllDbContext newHealthcare4AllDbContext) {
            UserName = newUserName;
            healthcare4AllDbContext = newHealthcare4AllDbContext;
        }

        protected string UserName { get; set; } = "";

        protected readonly Healthcare4AllDbContext healthcare4AllDbContext;

        public void GetProfile() { }

        public void EditProfile() { }
    }
}   