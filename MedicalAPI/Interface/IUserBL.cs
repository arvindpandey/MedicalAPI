using MedicalAPI.MedicalEntity;
using MedicalAPI.Model;

namespace MedicalAPI.Interface
{
    public interface IUserBL
    {
        IEnumerable<TblUser> GetAllUsers();
        IEnumerable<TblUser> GetUsers();
        TblUser? GetUserById(int id);
        void AddUser(TblUser userModel);
        void UpdateUser(TblUser userModel);
        void DeleteUser(int id);
    }
}
