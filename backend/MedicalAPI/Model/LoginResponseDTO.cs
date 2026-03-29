namespace MedicalAPI.Model
{
    public class LoginResponseDTO
    {
        public string Token { get; set; }
        public string FullName { get; set; }
        public string Role { get; set; }
        public int UserID { get; set; }
    }
}
