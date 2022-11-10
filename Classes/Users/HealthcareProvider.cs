﻿using HealthCare4All.Data;
using HealthCare4All.Data.HTTP;

namespace HealthCare4All.Classes.Users {
    public class HealthcareProvider : User {

        public HealthcareProvider(
            string newUserName,
            Healthcare4AllDbContext newHealthcare4AllDbContext) : base(newUserName, newHealthcare4AllDbContext) {

        }

        private List<int> GetPatientUserIdListFromUserName(string userName) {
            var patientUserIdQuery = from UserInfo in healthcare4AllDbContext.UserInfos
                                     where UserInfo.UserName == userName
                                     select UserInfo.UserId;

            return patientUserIdQuery.ToList();
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

        public void AddAppointment(ApiAppointment apiAppointment) {
            List<int> patientUserIds = GetPatientUserIdListFromUserName(apiAppointment.PatientUserName);

            if (patientUserIds.Count() == 1) {
                healthcare4AllDbContext.Appointments.Add(new Appointment {
                    AppointmentId = 0,
                    CreatorId = UserId,
                    PatientId = patientUserIds[0],
                    Time = apiAppointment.Time,
                    Street = apiAppointment.Street,
                    City = apiAppointment.City,
                    State = apiAppointment.State,
                    Postalcode = apiAppointment.Postalcode,
                    BulidingNumber = apiAppointment.BulidingNumber
                });

                try {
                    healthcare4AllDbContext.SaveChanges();
                } catch (Exception e) {

                }
            }
        }

        public void EditAppointment() { }
        
        public List<ApiTreatment> GetTreatments(string userName) {
            var treatmentQuery = from Treatment in healthcare4AllDbContext.Treatments
                                 join UserInfoPatient in healthcare4AllDbContext.UserInfos on Treatment.PatientId equals UserInfoPatient.UserId
                                 join UserInfoProvider in healthcare4AllDbContext.UserInfos on Treatment.PatientId equals UserInfoProvider.UserId
                                 join TreatmentTime in healthcare4AllDbContext.TreatmentTimes on Treatment.TreatmentId equals TreatmentTime.TreatmentId
                                 into TreatmentsWithAndWithoutTime
                                 from TreatmentTimeNull in TreatmentsWithAndWithoutTime.DefaultIfEmpty()
                                 where (UserInfoPatient.UserName == userName && UserInfoProvider.UserId == UserId)
                                 select new {
                                     TreatmentId = Treatment.TreatmentId,
                                     ProviderUserName = UserInfoProvider.UserName,
                                     ProviderFirstName = UserInfoProvider.FirstName,
                                     ProviderLastName = UserInfoProvider.LastName,
                                     PatientUserName = UserInfoPatient.UserName,
                                     PatientFirstName = UserInfoPatient.FirstName,
                                     PatientLastName = UserInfoPatient.LastName,
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
                        TreatmentId = treatmentFromQuery.TreatmentId,
                        ProviderUserName = treatmentFromQuery.ProviderUserName,
                        ProviderFirstName = treatmentFromQuery.ProviderFirstName,
                        ProviderLastName = treatmentFromQuery.ProviderLastName,
                        PatientUserName = treatmentFromQuery.PatientUserName,
                        PatientFirstName = treatmentFromQuery.PatientFirstName,
                        PatientLastName = treatmentFromQuery.PatientLastName,
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

    

        public void AddTreatment(ApiTreatment treatment) {
            List<int> patientUserIds = GetPatientUserIdListFromUserName(treatment.PatientUserName);
            Treatment dbTreatment;

            if (patientUserIds.Count() == 1) {
                dbTreatment = new Treatment {
                    TreatmentId = 0,
                    CreatorId = UserId,
                    PatientId = patientUserIds[0],
                    Name = treatment.Name,
                    Dose = treatment.Dose,
                    Comments = treatment.Comments,
                    IsPrescription = treatment.IsPrescription
                };

                healthcare4AllDbContext.Treatments.Add(dbTreatment);

                try {
                    healthcare4AllDbContext.SaveChanges();

                    if (treatment.Time != null) {
                        foreach (DateTime time in treatment.Time) {
                            healthcare4AllDbContext.TreatmentTimes.Add(new TreatmentTime {
                                TreatmentId = dbTreatment.TreatmentId,
                                Time = time
                            });
                        }
                    }

                    healthcare4AllDbContext.SaveChanges();
                } catch (Exception e) {

                }
            }
        }

        public void RemoveTreatment() { }

        public void EditTreatment() { }
    }
}
