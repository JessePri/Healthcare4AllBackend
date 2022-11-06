using System;
using System.Collections.Generic;

namespace HealthCare4All.Data
{
    public partial class Treatment
    {
        public int TreatmentId { get; set; }
        public int CreatorId { get; set; }
        public int PatientId { get; set; }
        public string Name { get; set; } = null!;
        public string? Dose { get; set; }
        public string? Comments { get; set; }
        public bool IsPrescription { get; set; }

        public virtual UserInfo Creator { get; set; } = null!;
    }
}
