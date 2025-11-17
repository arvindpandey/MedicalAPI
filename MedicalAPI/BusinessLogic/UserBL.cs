using MedicalAPI.Interface;
using MedicalAPI.MedicalEntity;

namespace MedicalAPI.BusinessLogic
{
    public class UserBL : IUserBL
    {
        public MedicalDbContext Usercontext;

        public UserBL(MedicalDbContext context)
        {
            Usercontext = context;

        }
        public IEnumerable<TblUser> GetAllUsers()
        {
            return Usercontext.TblUsers.ToList();
        }
        public IEnumerable<TblUser> GetUsers()
        {
            return Usercontext.TblUsers.Where(u => u.UserIsActive == true).ToList();
        }
        public TblUser? GetUserById(int id)
        {
            return Usercontext.TblUsers.FirstOrDefault(u => u.UserId == id);
        }
        public void AddUser(TblUser user)
        {
            Usercontext.TblUsers.Add(user);
            Usercontext.SaveChanges();
        }
        public void UpdateUser(TblUser user)
        {
            Usercontext.TblUsers.Update(user);
            Usercontext.SaveChanges();
        } 
        public void DeleteUser(int id)
        {
            var user = Usercontext.TblUsers.FirstOrDefault(u => u.UserId == id);
            if (user != null)
            {
                Usercontext.TblUsers.Remove(user);
                Usercontext.SaveChanges();
            }
        }   
    }
}
