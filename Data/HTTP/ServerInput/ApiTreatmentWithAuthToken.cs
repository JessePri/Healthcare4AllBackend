using HealthCare4All.Models;

namespace HealthCare4All.Data.HTTP.ServerInput {
    public class ApiTreatmentWithAuthToken : ApiTreatment {
        public string EncodedJwt { get; set; } = "";
    }
}
