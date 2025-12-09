using Management.Model.ResponseModel;
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
                var CheckAlreayExist = await unitOfWork.RepoMedicalDBContext.TblUsers.Where(x => x.UserMobileNo == _usermodel.UserMobileNo || x.UserAadharCard == _usermodel.UserAadharCard).FirstOrDefaultAsync();

                if (string.IsNullOrWhiteSpace(CheckAlreayExist.UserAadharCard) || CheckAlreayExist.UserMobileNo == 0)
                {

                    var entity = new TblUser
                    {
                        LoginName = _usermodel.Name,
                        UserPassword = Encryption.Encrypt(_usermodel.UserPassword),
                        UserFirstName = _usermodel.UserFirstName,
                        UserMiddleName = _usermodel.UserMiddleName,
                        UserLastName = _usermodel.UserLastName,
                        UserGender = _usermodel.UserGender,
                        UserRoleId = _usermodel.RoleID,
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
                else
                {
                    return "Record already Exist";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }

        //update   
        public async Task<string> Update(UserModel _usermodel)
        {
            try
            {
                var users = await medicalDbContext.TblUsers.FirstOrDefaultAsync(x => x.UserId == _usermodel.UserId);
                if (users == null)
                {
                    return "Record Not Found";
                }
                users.LoginName = _usermodel.Name;
                users.UserPassword = _usermodel.UserPassword;
                users.UserFirstName = _usermodel.UserFirstName;
                users.UserMiddleName = _usermodel.UserMiddleName;
                users.UserLastName = _usermodel.UserLastName;
                users.UserGender = _usermodel.UserGender;
                users.UserRoleId = _usermodel.RoleID;
                users.UserMobileNo = _usermodel.UserMobileNo;
                users.UserAge = _usermodel.UserAge;
                users.UserModifiedDate = DateTime.Now;
                users.UserAadharCard = _usermodel.UserAadharCard;
                users.UserIsActive = _usermodel.UserIsActive;

                await medicalDbContext.SaveChangesAsync();

                return "record Updated Successfully";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<string> DeleteRecord(int id)
        {
            var users = await medicalDbContext.TblUsers.FirstAsync(x => x.UserId == id);

            if (users == null)
            {
                return "Record Not Found";
            }
            medicalDbContext.Remove(users);
            await medicalDbContext.SaveChangesAsync();
            return "record Deleted Successfully";
        }

    }
    public interface IUserBL
    {
        Task<IEnumerable<UserModel>> GetAll();
        Task<String> Adds(UserModel _usermodel);
        Task<UserModel> GetAllByUserID(int ID);
        Task<string> Update(UserModel _usermodel);
        Task<string> DeleteRecord(int id);
    }
}
