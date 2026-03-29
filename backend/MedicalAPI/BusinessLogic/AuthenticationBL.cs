using Management.Model.ResponseModel;
using MedicalAPI.MedicalCore;
using MedicalAPI.MedicalEntity;
using MedicalAPI.Model;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt; 
 
using System.Security.Claims;
using System.Text;

namespace MedicalAPI.BusinessLogic
{
    public class AuthenticationBL : IAuthenticationBL
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly MedicalDbContext medicalDbContext;
        private IConfiguration _config;
       
        public AuthenticationBL(IUnitOfWork _unitOfWork, MedicalDbContext _medicalDbContext, IConfiguration config)
        {
            unitOfWork = _unitOfWork;
            medicalDbContext = _medicalDbContext;
            _config = config;
        }
        public async Task<AuthenticationResponseModel> Login(AuthenticationRequestModel authenticationRequestModel)
        {
            AuthenticationResponseModel objAuthRespo = new AuthenticationResponseModel();

            //var GetPassword = Encryption.Decrypt("iUHYR/TQwctWuP2fvleR54aM2nDyqf6cpVSLmXxA5Us=");

            var userExists = await unitOfWork.UserRepository.Any(x => x.UserIsActive == true && x.UserEmailId == authenticationRequestModel.Emaiid && x.UserPassword == Encryption.Encrypt(authenticationRequestModel.Password));

            if (!userExists)
            {
                objAuthRespo.StatusMessage = "Invalid user details";
            }
            else
            {
                var objectUser = await unitOfWork.UserRepository.FirstOrDefault(x => x.UserIsActive == true && x.UserEmailId == authenticationRequestModel.Emaiid && x.UserPassword == Encryption.Encrypt(authenticationRequestModel.Password));



                var isExpired = Encryption.IsPasswordExpired(objectUser.UserEmailId);

                if (isExpired)
                {
                    objAuthRespo.StatusCode = 200;
                    objAuthRespo.StatusMessage = "Password has been Expired";
                    return objAuthRespo;
                }
                else
                {
                    if (objectUser != null)
                    {
                        objAuthRespo.UserId = objectUser.UserId;
                        objAuthRespo.RoleID = Convert.ToInt32(objectUser.UserRoleId);
                        objAuthRespo.UserName = objectUser.LoginName;
                        objAuthRespo.IsActive = Convert.ToBoolean(objectUser.UserIsActive);
                        objAuthRespo.StatusCode = 200;
                        objAuthRespo.StatusMessage = "Login has been Successfully..";
                        objAuthRespo.Token = GenerateJSONWebToken(objectUser);


                    }
                }
            }
            return objAuthRespo;


        } 
        private string GenerateJSONWebToken(TblUser userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config[""]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                        new Claim(JwtRegisteredClaimNames.Sub, userInfo.LoginName),
                        new Claim(JwtRegisteredClaimNames.Email, userInfo.UserEmailId),
                        new Claim("UserId", userInfo.UserId.ToString())
                    };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              //claims.ToArray(),
              expires: DateTime.Now.AddHours(30),
              signingCredentials: credentials
              );

            return new JwtSecurityTokenHandler().WriteToken(token);


        }

    }
    public interface IAuthenticationBL
    {
         Task<AuthenticationResponseModel> Login(AuthenticationRequestModel authenticationRequestModel);
    }
}
