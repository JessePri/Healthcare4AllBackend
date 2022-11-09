using HealthCare4All.Data;
using HealthCare4All.Data.HTTP;
using HealthCare4All.Models;

namespace HealthCare4All.Classes.Users {
    public class Patient : User {
        public Patient(
            string newUserName,
            Healthcare4AllDbContext newHealthcare4AllDbContext) : base(newUserName, newHealthcare4AllDbContext) {

        }
        public List<ApiAppointment> GetAppointments() {
            var appointmentQuery = from Appointment in healthcare4AllDbContext.Appointments
                                   join UserInfoPatient in healthcare4AllDbContext.UserInfos on Appointment.PatientId equals UserInfoPatient.UserId
                                   join UserInfoProvider in healthcare4AllDbContext.UserInfos on Appointment.PatientId equals UserInfoProvider.UserId
                                   where Appointment.PatientId == UserId
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

        public List<ApiTreatment> GetTreatments() {
            var treatmentQuery = from Treatment in healthcare4AllDbContext.Treatments
                                 join UserInfoProvider in healthcare4AllDbContext.UserInfos on Treatment.CreatorId equals UserInfoProvider.UserId
                                 join TreatmentTime in healthcare4AllDbContext.TreatmentTimes on Treatment.TreatmentId equals TreatmentTime.TreatmentId
                                 into TreatmentsWithAndWithoutTime
                                 from TreatmentTimeNull in TreatmentsWithAndWithoutTime.DefaultIfEmpty()
                                 where Treatment.PatientId == UserId
                                 orderby Treatment.TreatmentId ascending
                                 select new {
                                     TreatmentId = Treatment.TreatmentId,
                                     ProviderUserName = UserInfoProvider.UserName,
                                     ProviderFirstName = UserInfoProvider.FirstName,
                                     ProviderLastName = UserInfoProvider.LastName,
                                     Name = Treatment.Name,
                                     Dose = Treatment.Dose,
                                     Comments = Treatment.Comments,
                                     IsPrescription = Treatment.IsPrescription,
                                     Time = TreatmentTimeNull.Time
                                 };
            var treatmentsFromQuery = treatmentQuery.ToArray();
            int prevTreatmentID = int.MinValue;

            List<ApiTreatment> treatments = new List<ApiTreatment>();

            foreach (var treatmentFromQuery in treatmentsFromQuery) {
                if (treatmentFromQuery.TreatmentId != prevTreatmentID) {
                    treatments.Add(new ApiTreatment {
                        TreatmentID = treatmentFromQuery.TreatmentId,
                        ProviderUserName = treatmentFromQuery.ProviderUserName,
                        ProviderFirstName = treatmentFromQuery.ProviderFirstName,
                        ProviderLastName = treatmentFromQuery.ProviderLastName,
                        Name = treatmentFromQuery.Name,
                        Dose = treatmentFromQuery.Dose,
                        Comments = treatmentFromQuery.Comments,
                        IsPrescription = treatmentFromQuery.IsPrescription,
                        Time = new List<DateTime>()
                    });

                    if (treatmentFromQuery.Time != null) {
                        treatments.Last().Time.Add(treatmentFromQuery.Time);
                    }

                    prevTreatmentID = treatmentFromQuery.TreatmentId;
                } else {
                    treatments.Last().Time.Add(treatmentFromQuery.Time);
                }
            }

            return treatments;
        }

    }
}
