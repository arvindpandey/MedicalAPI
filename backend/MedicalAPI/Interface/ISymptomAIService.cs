using MedicalAPI.Model;

namespace MedicalAPI.Interface
{
    public interface ISymptomAIService
    {
        Task<SymptomAIResponse> AnalyzeSymptomsAsync(SymptomAIRequest request);
    }
}
