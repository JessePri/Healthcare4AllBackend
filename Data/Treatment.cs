using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HealthCare4All.Data
{
    public partial class Treatment
    {
        [Key]
        public int TreatmentId { get; set; }
        public int CreatorId { get; set; }
        public int PatientId { get; set; }
        public string Name { get; set; } = null!;
        public string? Dose { get; set; }
        public string? Comments { get; set; }
        public bool IsPrescription { get; set; }
    }
}
