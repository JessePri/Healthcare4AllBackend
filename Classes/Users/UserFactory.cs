using HealthCare4All.Data;
using HealthCare4All.Models;

namespace HealthCare4All.Classes.Users {
    public class UserFactory {
        public static User? Create(AuthToken token, Healthcare4AllDbContext newHealthcare4AllDbContext) { 
            switch (token.Privilege) {
                case UserLogin.HEALTHCARE_PROVIDER_PRIVILEGE_LEVEL:
                    return new HealthcareProvider(token.UserName, newHealthcare4AllDbContext);
                case UserLogin.USER_PRIVILEGE_LEVEL:
                    return new Patient(token.UserName, newHealthcare4AllDbContext);
                default:
                    return null;
            }
        }
    }
}
