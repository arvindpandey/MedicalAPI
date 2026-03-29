namespace MedicalAPI.Model
{
    public class UserModel
    {
        public string UserId { get; set; }

        public string? LoginName { get; set; }

        public string? UserPassword { get; set; }

        public string? UserFirstName { get; set; }

        public string? UserMiddleName { get; set; }

        public string? UserLastName { get; set; }
        public string? UserEmailId { get; set; }     

        public string? RoleID { get; set; }

        public string? UserGender { get; set; }

        public string? UserMobileNo { get; set; }

        public string? UserAge { get; set; }

        public string? UserAadharCard { get; set; }

        public string? UserIsActive { get; set; }

        public string? UserCreateDate { get; set; }

        public string? UserModifiedDate { get; set; }
    }
    public class UserResponseDTO
    {
        public int UserID { get; set; }
        public string FullName { get; set; }
        public string LoginName { get; set; }
        public string UserEmailID { get; set; }
        public string UserMobileNo { get; set; }
        public string UserGender { get; set; }
        public int? UserAge { get; set; }
        public bool UserIsActive { get; set; }
        public string RoleName { get; set; }
        public int UserRoleID { get; set; }
    }
    public class CreateUserDTO
    {
        public int UserRoleID { get; set; }
        public string LoginName { get; set; }
        public string Password { get; set; }  // Plain text → hashed in service
        public string UserFirstName { get; set; }
        public string UserMiddleName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmailID { get; set; }
        public string UserGender { get; set; }
        public string UserMobileNo { get; set; }
        public int? UserAge { get; set; }
        public string UserAadharCard { get; set; }
    }


}
