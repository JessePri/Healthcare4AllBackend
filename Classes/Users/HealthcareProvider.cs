using HealthCare4All.Data;
using HealthCare4All.Data.HTTP;

namespace HealthCare4All.Classes.Users {
    public class HealthcareProvider : User {

        public HealthcareProvider(
            string newUserName,
            Healthcare4AllDbContext newHealthcare4AllDbContext) : base(newUserName, newHealthcare4AllDbContext) {

        }

        public List<ApiAppointment> GetAppointments(string userName) {
            var appointmentQuery = from Appointment in healthcare4AllDbContext.Appointments
                                   join UserInfoPatient in healthcare4AllDbContext.UserInfos on Appointment.PatientId equals UserInfoPatient.UserId
                                   join UserInfoProvider in healthcare4AllDbContext.UserInfos on Appointment.PatientId equals UserInfoProvider.UserId
                                   where (UserInfoPatient.UserName == userName && UserInfoProvider.UserId == UserId)
                                   select new ApiAppointment {
                                       AppointmentId = Appointment.AppointmentId,
                                       ProviderUserName = UserInfoProvider.UserName,
                                       ProviderFirstName = UserInfoProvider.FirstName,
                                       ProviderLastName = UserInfoProvider.LastName,
                                       PatientUserName = UserInfoPatient.UserName,
                                       PatientFirstName = UserInfoPatient.FirstName,
                                       PatientLastName = UserInfoPatient.LastName,
                                       Time = Appointment.Time,
                                       Street = Appointment.Street,
                                       City = Appointment.City,
                                       State = Appointment.State,
                                       Postalcode = Appointment.Postalcode,
                                       BulidingNumber = Appointment.BulidingNumber
                                   };

            List<ApiAppointment> appointments = appointmentQuery.ToList();

            return appointments;
        }

        public void InsertTreatment() { }

        public void RemoveTreatment() { }

        public void EditTreatment() { }
    }
}
