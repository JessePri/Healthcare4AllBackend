using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare4All.Data
{
    public partial class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }
        public int CreatorId { get; set; }
        public int PatientId { get; set; }
        public DateTime Time { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Postalcode { get; set; } = null!;
        public string BulidingNumber { get; set; } = null!;
    }
}
