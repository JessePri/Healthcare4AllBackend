using System;
using System.Collections.Generic;

namespace HealthCare4All.Data
{
    public partial class TreatmentTime
    {
        public int TreatmentId { get; set; }
        public DateTime Time { get; set; }

        public virtual Treatment Treatment { get; set; } = null!;
    }
}
