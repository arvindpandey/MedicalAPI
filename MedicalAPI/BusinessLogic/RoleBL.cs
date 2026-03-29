using MedicalAPI.MedicalCore;
using MedicalAPI.MedicalEntity;
using MedicalAPI.Model;
using Microsoft.EntityFrameworkCore;

namespace MedicalAPI.BusinessLogic
{
    public class RoleBL : IUserRole
    {
        
        private readonly MedicalDbContext medicalDbContext;

        public RoleBL(MedicalDbContext _medicalDbContext)
        { 
            medicalDbContext = _medicalDbContext;
        }

        public async Task<IEnumerable<UserRoleModel>> GetRolesDetails()
        {
            var GetValue = await medicalDbContext.UserRoles.ToListAsync();

            return GetValue.Select(ur => new UserRoleModel
            {
                Urid = ur.Urid,
                RoleName = ur.RoleName,
                
            });
        } 
    }
    public interface IUserRole
    {
        Task<IEnumerable<UserRoleModel>> GetRolesDetails();
    }


}
