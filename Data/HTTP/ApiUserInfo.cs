namespace HealthCare4All.Data.HTTP {
    public class ApiUserInfo {
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime BirthDay { get; set; }
    }
}
