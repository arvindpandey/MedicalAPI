using Management.Model.ResponseModel;
using MedicalAPI.Interface;
using MedicalAPI.MedicalCore;
using MedicalAPI.MedicalEntity;
using MedicalAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MedicalAPI.BusinessLogic
{
    public class UserBL : IUserService
    {

        private readonly MedicalDbContext _context;
        private readonly IConfiguration _config;  
        public UserBL(MedicalDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        public async Task<IEnumerable<UserResponseDTO>> GetAllUsersAsync()
        {
             
            return await _context.TblUsers.Include(u=>u.UserRole)
                .Where(u => Convert.ToBoolean(u.UserIsActive))
                .Select(u => new UserResponseDTO
                {
                    UserID = u.UserId,
                    FullName = $"{u.UserFirstName} {u.UserLastName}",
                    LoginName = u.LoginName,
                    UserEmailID = u.UserEmailId,
                    UserMobileNo = u.UserMobileNo,
                    UserGender = u.UserGender,
                    UserAge = u.UserAge,
                    UserIsActive = Convert.ToBoolean(u.UserIsActive),
                    RoleName = u.UserRole.RoleName,
                    UserRoleID = u.UserRoleId
                })
                .ToListAsync();
        }

        public async Task<UserResponseDTO> GetUserByIdAsync(int userId)
        {
            var u = await _context.TblUsers
                .Include(x => x.UserRole)
                .FirstOrDefaultAsync(x => x.UserId == userId);

            if (u == null) return null;

            return new UserResponseDTO
            {
                UserID = u.UserId,
                FullName = $"{u.UserFirstName} {u.UserLastName}",
                LoginName = u.LoginName,
                UserEmailID = u.UserEmailId,
                UserMobileNo = u.UserMobileNo,
                UserGender = u.UserGender,
                UserAge = u.UserAge,
                UserIsActive = Convert.ToBoolean(u.UserIsActive),
                RoleName = u.UserRole.RoleName,
                UserRoleID = u.UserRoleId
            };
        }

        public async Task<int> CreateUserAsync(CreateUserDTO dto, int createdByUserId)
        {
             
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            var user = new TblUser
            {
                UserRoleId = dto.UserRoleID,
                LoginName = dto.LoginName,
                UserPassword = hashedPassword,
                UserFirstName = dto.UserFirstName,
                UserMiddleName = dto.UserMiddleName,
                UserLastName = dto.UserLastName,
                UserEmailId = dto.UserEmailID,
                UserGender = dto.UserGender,
                UserMobileNo = dto.UserMobileNo,
                UserAge = dto.UserAge,
                UserAadharCard = dto.UserAadharCard,
                UserIsActive = true,
                UserCreateDate = DateTime.Now
            };

            _context.TblUsers.Add(user);
            await _context.SaveChangesAsync();
            return user.UserId;
        }

        public async Task UpdateUserAsync(int userId, CreateUserDTO dto)
        {
            var user = await _context.TblUsers.FindAsync(userId);
            if (user == null) throw new Exception("User not found");

            user.UserFirstName = dto.UserFirstName;
            user.UserMiddleName = dto.UserMiddleName;
            user.UserLastName = dto.UserLastName;
            user.UserEmailId = dto.UserEmailID;
            user.UserGender = dto.UserGender;
            user.UserMobileNo = dto.UserMobileNo;
            user.UserAge = dto.UserAge;
            user.UserAadharCard = dto.UserAadharCard;
            user.UserRoleId = dto.UserRoleID;
            user.UserModifiedDate = DateTime.Now;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteUserAsync(int userId)
        {
            var user = await _context.TblUsers.FindAsync(userId);
            if (user == null) throw new Exception("User not found"); 
            user.UserIsActive = false;
            user.UserModifiedDate = DateTime.Now;
            await _context.SaveChangesAsync();
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginDTO dto)
        {
            var user = await _context.TblUsers
                .Include(u => u.UserRole)
                .FirstOrDefaultAsync(u => u.LoginName == dto.LoginName && Convert.ToBoolean(u.UserIsActive));

            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.UserPassword))
                throw new UnauthorizedAccessException("Invalid credentials");

            // Generate JWT token
            var token = GenerateJwtToken(user);

            return new LoginResponseDTO
            {
                Token = token,
                FullName = $"{user.UserFirstName} {user.UserMiddleName}",
                Role = user.UserRole.RoleName,
                UserID = user.UserId
            };
        }

         
        private string GenerateJwtToken(TblUser user)
        {
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim(ClaimTypes.Name, user.LoginName),
                new Claim(ClaimTypes.Role, user.UserRole.RoleName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(8),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
     
}
