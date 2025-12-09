namespace MedicalAPI.Model
{
    public class UserModel
    {
        public int UserId { get; set; }

        public string? Name { get; set; }

        public string? UserPassword { get; set; }

        public string? UserFirstName { get; set; }

        public string? UserMiddleName { get; set; }

        public string? UserLastName { get; set; }

        public int? RoleID { get; set; }

        public string? UserGender { get; set; }

        public long? UserMobileNo { get; set; }

        public int? UserAge { get; set; }

        public string? UserAadharCard { get; set; }

        public bool? UserIsActive { get; set; }

        public DateTime? UserCreateDate { get; set; }

        public DateTime? UserModifiedDate { get; set; }
    }
}
