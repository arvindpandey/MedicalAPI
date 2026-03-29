namespace MedicalAPI.Model
{
    public class SymptomAIRequest
    {
        public int PatientId { get; set; }      
        public List<string> Symptoms { get; set; } = new();
        public int? PatientAge { get; set; }  
        public string? AdditionalNotes { get; set; }
    }
}
