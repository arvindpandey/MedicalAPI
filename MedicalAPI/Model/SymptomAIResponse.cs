namespace MedicalAPI.Model
{
    public class SymptomAIResponse
    {
        public bool Success { get; set; }
        public string AIAdvice { get; set; } = string.Empty;
        public string Severity { get; set; } = string.Empty;        // Mild / Moderate / Severe
        public string SuggestedAction { get; set; } = string.Empty; // Rest / See doctor / Emergency
        public bool ServedFromCache { get; set; }                   // true = came from DB, false = fresh AI call
        public string? Error { get; set; }
    }
}
