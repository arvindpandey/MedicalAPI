namespace MedicalAPI.Model
{
    public class SymptomResponseDTO
    {
        public int SymptomID { get; set; }
        public int PatientID { get; set; }
        public string PatientName { get; set; }
        public string SymptomDetails { get; set; }
        public string RecordedBy { get; set; }
    }
}
