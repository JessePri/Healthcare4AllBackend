using System.Runtime.ConstrainedExecution;

namespace HealthCare4All.Models {
    public class UserLogin {
        public const int USER_PRIVILEGE_LEVEL = 0b00000001;
        public const int HEALTHCARE_PROVIDER_PRIVILEGE_LEVEL = 0b00000010;
        public const int ADMIN_PRIVILEGE_LEVEL = 0b10000000;

        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public int Privilege { get; set; }
    }
}
