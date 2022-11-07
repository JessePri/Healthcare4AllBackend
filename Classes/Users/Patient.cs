using HealthCare4All.Data;
using HealthCare4All.Models;

namespace HealthCare4All.Classes.Users {
    public class Patient : User {
        public Patient(
            string newUserName,
            Healthcare4AllDbContext newHealthcare4AllDbContext) : base(newUserName, newHealthcare4AllDbContext) {

        }
        public Appointment[] GetAppointments() {
            var appointmentQuery = from Appointment in healthcare4AllDbContext.Appointments
                                   where Appointment.PatientId == UserId select Appointment;

            Appointment[] appointments = appointmentQuery.ToArray();

            return appointments;
        }

        public void GetTreatMents() { }
    }
}
