using HealthCare4All.Models;

namespace HealthCare4All.Data.HTTP.ServerInput {
    public class ApiTreatmentWithAuthToken : ApiTreatment {
        public AuthToken Token { get; set; }
    }
}
