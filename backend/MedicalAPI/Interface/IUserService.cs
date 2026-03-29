using MedicalAPI.Model;

namespace MedicalAPI.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync();
        Task<UserResponseDTO> GetUserByIdAsync(int userId);
        Task<int> CreateUserAsync(CreateUserDTO dto, int createdByUserId);
        Task UpdateUserAsync(int userId, CreateUserDTO dto);
        Task DeleteUserAsync(int userId);
        Task<LoginResponseDTO> LoginAsync(LoginDTO dto);
    }
}
