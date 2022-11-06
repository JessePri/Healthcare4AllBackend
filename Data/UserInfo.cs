using System;
using System.Collections.Generic;

namespace HealthCare4All.Data
{
    public partial class UserInfo
    {
        public UserInfo()
        {
            Appointments = new HashSet<Appointment>();
            Treatments = new HashSet<Treatment>();
        }

        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public DateTime BirthDay { get; set; }
        public int MaxPriviledge { get; set; }

        public virtual ICollection<Appointment> Appointments { get; set; }
        public virtual ICollection<Treatment> Treatments { get; set; }
    }
}
