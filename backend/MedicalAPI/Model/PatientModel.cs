namespace MedicalAPI.Model
{
    // Patient DTOs
    public class CreatePatientDTO
    {
        public string PatientName { get; set; }
        public string PatientGeneder { get; set; }
        public string Address { get; set; }
        public string BloodGroup { get; set; }
        public int? Age { get; set; }
        public decimal? Weight { get; set; }
    }
    public class PatientResponseDTO
    {
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public string PatientGeneder { get; set; }
        public string Address { get; set; }
        public string BloodGroup { get; set; }
        public int? Age { get; set; }
        public decimal? Weight { get; set; }
        public string EntryDate { get; set; }
        public string CreatedByUser { get; set; }
    }

}

