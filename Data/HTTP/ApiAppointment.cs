namespace HealthCare4All.Data.HTTP {
    public class ApiAppointment {
        public int AppointmentId { get; set; }
        public string ProviderUserName { get; set; } = null!;
        public string ProviderFirstName { get; set; } = null!;
        public string ProviderLastName { get; set; } = null!;
        public string PatientUserName { get; set; } = null!;
        public string PatientFirstName { get; set; } = null!;
        public string PatientLastName { get; set; } = null!;
        public DateTime Time { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string State { get; set; } = null!;
        public string Postalcode { get; set; } = null!;
        public string BulidingNumber { get; set; } = null!;
    }
}
