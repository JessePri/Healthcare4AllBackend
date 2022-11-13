namespace HealthCare4All.Models {
    /**
     * This is a temporary authorization token that the android application will store somehow. This will be replaced by the second sprint with a JWT token.
     */
    public class AuthToken {
        public string UserName { get; set; } = "";
        public int Privilege { get; set; }
    }
}
