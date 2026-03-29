using MedicalAPI.Model;

namespace MedicalAPI.Interface
{
    public interface IPatientService
    {
        Task<IEnumerable<PatientResponseDTO>> GetAllPatientsAsync();
        Task<PatientResponseDTO> GetPatientByIdAsync(int id);
        Task<int> CreatePatientAsync(CreatePatientDTO dto, int userId);
        Task UpdatePatientAsync(int id, CreatePatientDTO dto);
        Task DeletePatientAsync(int id);
    }
}
