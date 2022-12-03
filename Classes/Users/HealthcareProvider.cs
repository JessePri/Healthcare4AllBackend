using HealthCare4All.Data;
using HealthCare4All.Data.HTTP;
using Microsoft.OpenApi.Models;
using System.Transactions;

namespace HealthCare4All.Classes.Users
{
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

        /**
         * @brief This will return the whole ApiAppointment object which will then be serialized to JSON as shown in the Swagger web page
         *      when this is run in debug mode. 
         * @param[input] string This is the user name of the patient that has appointments with the doctor.
         * @return A list of fully filled out ApiAppointment objects. 
         */
        public List<ApiAppointment> GetAppointments(string userName) {
            var appointmentQuery = from Appointment in healthcare4AllDbContext.Appointments
                                   join UserInfoPatient in healthcare4AllDbContext.UserInfos on Appointment.PatientId equals UserInfoPatient.UserId
                                   join UserInfoProvider in healthcare4AllDbContext.UserInfos on Appointment.CreatorId equals UserInfoProvider.UserId
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

        /**
         * @brief This will add an appointment with the doctor and another user.
         * @param[input] apiAppointment The only fields that matter are the location and time related ones and the userName section. 
         *      All other fields will be ignored.
         */
        public void AddAppointment(ApiAppointment apiAppointment) {
            List<int> patientUserIds = GetPatientUserIdListFromUserName(apiAppointment.PatientUserName);

            if (patientUserIds.Count == 1) {
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

        /**
         * @brief This will edit an appointment and functions effectively like a delete and then re-add. 
         * @param[input] apiAppointment The only fields that matter are the location and time related ones and the userName section. 
         *      All other fields will be ignored.
         */
        public void EditAppointment(ApiAppointment apiAppointment) {
            Appointment? appointment = healthcare4AllDbContext.Appointments.Find(apiAppointment.AppointmentId);

            if (appointment != null && appointment.CreatorId == UserId) {
                appointment.Time = apiAppointment.Time;
                appointment.Street = apiAppointment.Street;
                appointment.City = apiAppointment.City;
                appointment.State = apiAppointment.State;
                appointment.Postalcode = apiAppointment.Postalcode;
                appointment.BulidingNumber = apiAppointment.BulidingNumber;

                try {
                    healthcare4AllDbContext.SaveChanges();
                } catch (Exception e) {

                }
            }
        }

        /**
         * @brief Removes an appointment according to the appointmentId int the apiAppointment field. Will only do so if the healthcare provider
         *      has created this appointment in the first place.
         * @param[input] apiAppointment The apiAppointment class that only needs the appointmentId field filled. 
         */
        public void RemoveAppointment(ApiAppointment apiAppointment) {
            Appointment? appointment = healthcare4AllDbContext.Appointments.Find(apiAppointment.AppointmentId);

            if (appointment == null || appointment.CreatorId != UserId) {
                return;
            }

            healthcare4AllDbContext.Appointments.Attach(appointment);

            if (appointment.CreatorId != UserId) {
                return;
            }

            healthcare4AllDbContext.Appointments.Remove(appointment);

            try {
                healthcare4AllDbContext.SaveChanges();
            } catch (Exception e) {

            }
        }
        
        /**
         * @brief This will get all treatments that are associated with the user and are created by the healthcare provider. 
         * @param[input] userName The userName of the patient associated with the providers treatment.
         * @return Returns all treatments that are associated with a health care provider and the user name entered.
         */
        public List<ApiTreatment> GetTreatments(string userName) {
            var treatmentQuery = from Treatment in healthcare4AllDbContext.Treatments
                                 join UserInfoPatient in healthcare4AllDbContext.UserInfos on Treatment.PatientId equals UserInfoPatient.UserId
                                 join UserInfoProvider in healthcare4AllDbContext.UserInfos on Treatment.CreatorId equals UserInfoProvider.UserId
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

        /**
         * @brief Adds a treatment to a user from a healthcare provider.
         * @param[input] treatment ApiTreatment object where only the PatientUserName and all other treatment specific information matter.
         */
        public void AddTreatment(ApiTreatment treatment) {
            List<int> patientUserIds = GetPatientUserIdListFromUserName(treatment.PatientUserName);
            Treatment dbTreatment;

            if (patientUserIds.Count == 1) {
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

        /**
         * @brief Edits a treatment to a user from a healthcare provider (works like a delete and re-add).
         * @param[input] treatment ApiTreatment object where only the PatientUserName and all other treatment specific information matter.
         */
        public void EditTreatment(ApiTreatment apiTreatment) {
            Treatment? treatment = healthcare4AllDbContext.Treatments.Find(apiTreatment.TreatmentId);

            List<TreatmentTime> treatmentTimes;
            List<TreatmentTime> newTreatmentTimes = new List<TreatmentTime>();

            if (treatment != null && treatment.CreatorId == UserId) {
                treatment.Name = apiTreatment.Name;
                treatment.Dose = apiTreatment.Dose;
                treatment.Comments = apiTreatment.Comments;
                treatment.IsPrescription = apiTreatment.IsPrescription;

                var treatmentTimeQuery = from TreatmentTime in healthcare4AllDbContext.TreatmentTimes
                                         where TreatmentTime.TreatmentId == apiTreatment.TreatmentId
                                         select TreatmentTime;

                treatmentTimes = treatmentTimeQuery.ToList();
                

                healthcare4AllDbContext.TreatmentTimes.AttachRange(treatmentTimes);
                healthcare4AllDbContext.TreatmentTimes.RemoveRange(treatmentTimes);

                if (apiTreatment.Time != null) {
                    foreach (DateTime time in apiTreatment.Time) {
                        newTreatmentTimes.Add(new TreatmentTime {
                            TreatmentId = apiTreatment.TreatmentId,
                            Time = time
                        });
                    }
                }

                healthcare4AllDbContext.TreatmentTimes.AddRange(newTreatmentTimes);

                try {
                    healthcare4AllDbContext.SaveChanges();
                } catch (Exception e) {

                }
            }
        }

        /**
         * @brief Removes a treatment based on the TreatmentId part of the ApiTreatment object.
         * @param[input] apiTreatment The treatment where only the TreatmentId field matters. 
         */
        public void RemoveTreatment(ApiTreatment apiTreatment) {
            Treatment? treatment = healthcare4AllDbContext.Treatments.Find(apiTreatment.TreatmentId);

            if (treatment == null || treatment.CreatorId != UserId) {
                return;
            }

            var treatmentTimeQuery = from TreatmentTime in healthcare4AllDbContext.TreatmentTimes
                                     where TreatmentTime.TreatmentId == apiTreatment.TreatmentId
                                     select TreatmentTime;

            List<TreatmentTime> treatmentTimes = treatmentTimeQuery.ToList();

            healthcare4AllDbContext.TreatmentTimes.AttachRange(treatmentTimes);
            healthcare4AllDbContext.TreatmentTimes.RemoveRange(treatmentTimes);
            healthcare4AllDbContext.Treatments.Remove(treatment);

            try {
                healthcare4AllDbContext.SaveChanges();
            } catch (Exception e) {

            }
        }
    }
}
