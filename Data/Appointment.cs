using System;
using System.Collections.Generic;

namespace HealthCare4All.Data
{
    public partial class Appointment
    {
        public int AppointmentId { get; set; }
        public int CreatorId { get; set; }
        public int PatientId { get; set; }
        public DateTime Time { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Postalcode { get; set; } = null!;
        public string BulidingNumber { get; set; } = null!;

        public virtual UserInfo Creator { get; set; } = null!;
    }
}
