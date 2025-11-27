using MedicalAPI.MedicalCore;
using MedicalAPI.MedicalEntity;
using MedicalAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace MedicalAPI.BusinessLogic
{
    public class UserBL : IUserBL
    {

        private readonly IUnitOfWork unitOfWork;
        private readonly MedicalDbContext medicalDbContext;
        public UserBL(IUnitOfWork _unitOfWork, MedicalDbContext _medicalDbContext)
        {
            unitOfWork = _unitOfWork;
            medicalDbContext = _medicalDbContext;

        }
        //Get all UserDetails
        public async Task<IEnumerable<UserModel>> GetAll()
        {
            var users = await unitOfWork.RepoMedicalDBContext.TblUsers.ToListAsync();

            return users.Select(u => new UserModel
            {
                UserId = u.UserId,
                UserFirstName = u.UserFirstName,
                UserMiddleName = u.UserMiddleName,
                UserLastName = u.UserLastName,
                UserGender = u.UserGender,
                UserMobileNo = u.UserMobileNo,
                UserAge = u.UserAge,
                UserAadharCard = u.UserAadharCard,
                UserIsActive = u.UserIsActive,
                UserCreateDate = u.UserCreateDate,
                UserModifiedDate = u.UserModifiedDate
            });
        }
        //Get UserData by ID : 
        public async Task<UserModel> GetAllByUserID(int ID)
        {
            var users = await unitOfWork.RepoMedicalDBContext.TblUsers.FirstOrDefaultAsync(x => x.UserId == ID);


            if (users == null)
                return null;

            return new UserModel
            {
                UserId = users.UserId,
                UserFirstName = users.UserFirstName,
                UserMiddleName = users.UserMiddleName,
                UserLastName = users.UserLastName,
                UserGender = users.UserGender,
                UserMobileNo = users.UserMobileNo,
                UserAge = users.UserAge,
                UserAadharCard = users.UserAadharCard,
                UserIsActive = users.UserIsActive,
                UserCreateDate = users.UserCreateDate,
                UserModifiedDate = users.UserModifiedDate,
            };
        }

        //Insert User Details
        public async Task<string> Adds(UserModel _usermodel)
        {

            try
            {
                var entity = new TblUser
                {
                    LoginName = _usermodel.Name,
                    UserPassword = _usermodel.UserPassword,
                    UserFirstName = _usermodel.UserFirstName,
                    UserMiddleName = _usermodel.UserMiddleName,
                    UserLastName = _usermodel.UserLastName,
                    UserGender = _usermodel.UserGender,
                    UserSpecialization = _usermodel.UserSpecialization,
                    UserMobileNo = _usermodel.UserMobileNo,
                    UserAge = _usermodel.UserAge,
                    UserAadharCard = _usermodel.UserAadharCard,
                    UserIsActive = _usermodel.UserIsActive,
                    UserCreateDate = DateTime.Now
                };

                medicalDbContext.Add(entity);
                await medicalDbContext.SaveChangesAsync();

                return "Record saved successfully";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
         
    }
    public interface IUserBL
    {
        Task<IEnumerable<UserModel>> GetAll();
        Task<String> Adds(UserModel _usermodel);
        Task<UserModel> GetAllByUserID(int ID);
    }
}
