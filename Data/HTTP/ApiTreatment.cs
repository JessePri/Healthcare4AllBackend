﻿using System.Collections;

namespace HealthCare4All.Data.HTTP {
    public class ApiTreatment {
        public int TreatmentId { get; set; }
        public string ProviderUserName { get; set; } = null!;
        public string ProviderFirstName { get; set; } = null!;
        public string ProviderLastName { get; set; } = null!;
        public string PatientUserName { get; set; } = null!;
        public string PatientFirstName { get; set; } = null!;
        public string PatientLastName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string? Dose { get; set; }
        public string? Comments { get; set; }
        public bool IsPrescription { get; set; }
        public List<DateTime>? Time { get; set; }
    }
}
