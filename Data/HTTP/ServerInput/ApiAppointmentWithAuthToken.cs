﻿using HealthCare4All.Data.HTTP;
using HealthCare4All.Models;

namespace HealthCare4All.Data.HTTP.ServerInput
{
    public class ApiAppointmentWithAuthToken : ApiAppointment
    {
        public AuthToken Token { get; set; }
    }
}
