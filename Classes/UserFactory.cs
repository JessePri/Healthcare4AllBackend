using HealthCare4All.Data;
using HealthCare4All.Models;
using System.Security.Claims;

namespace HealthCare4All.Classes.Users
{
    public class UserFactory {
        public static User Create(ClaimsPrincipal claimsPrincipal, Healthcare4AllDbContext newHealthcare4AllDbContext) {
            string userName = "";
            int privledge = 0;
            
            foreach (Claim claim in claimsPrincipal.Claims) {
                if (claim.Type == "UserName") {
                    userName = claim.Value;
                } else if (claim.Type == "Privilege") {
                    privledge = int.Parse(claim.Value);
                    break;
                }
            }


            switch (privledge) {
                case UserLogin.HEALTHCARE_PROVIDER_PRIVILEGE_LEVEL:
                    return new HealthcareProvider(userName, newHealthcare4AllDbContext);
                case UserLogin.USER_PRIVILEGE_LEVEL:
                    return new Patient(userName, newHealthcare4AllDbContext);
                default:
                    return new NullUser();
            }
        }

    }
}
