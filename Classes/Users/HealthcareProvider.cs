using HealthCare4All.Data;

namespace HealthCare4All.Classes.Users {
    public class HealthcareProvider : User {

        public HealthcareProvider(
            string newUserName,
            Healthcare4AllDbContext newHealthcare4AllDbContext) : base(newUserName, newHealthcare4AllDbContext) {

        }
        public void InsertTreatment() { }

        public void RemoveTreatment() { }

        public void EditTreatment() { }
    }
}
