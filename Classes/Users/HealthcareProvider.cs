using HealthCare4All.Data;

namespace HealthCare4All.Classes.Users {
    public class HealthcareProvider : User {

        public HealthcareProvider(
            string newUserName,
            Healthcare4AllDbContext newHealthcare4AllDbContext) : base(newUserName, newHealthcare4AllDbContext) {

        }

        public Appointment[] GetAppointments(string userName) {
            var appointmentQuery = from Appointment in healthcare4AllDbContext.Appointments
                                   join UserInfo in healthcare4AllDbContext.UserInfos on Appointment.PatientId equals UserInfo.UserId
                                   where UserInfo.UserName == userName
                                   select Appointment;

            Appointment[] appointments = appointmentQuery.ToArray();

            return appointments;
        }

        public void InsertTreatment() { }

        public void RemoveTreatment() { }

        public void EditTreatment() { }
    }
}
