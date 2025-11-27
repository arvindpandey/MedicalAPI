using MedicalAPI.MedicalCore.Repository;
using MedicalAPI.MedicalEntity;

namespace MedicalAPI.MedicalCore
{
    public class UnitOfWork : IUnitOfWork
    { 
        public MedicalDbContext RepoMedicalDBContext { set; get; } 
        public IRepositoryBase<TblUser> UserRepository { set; get; } 

        public UnitOfWork(MedicalDbContext _RepoMedicalDBContext, IRepositoryBase<TblUser> _UserRepository)
        {
            RepoMedicalDBContext = _RepoMedicalDBContext;
            UserRepository = _UserRepository;
            
        }
        public async Task Commit()
        {
            RepoMedicalDBContext.SaveChanges();
        }

    }
    public interface IUnitOfWork
    {
        MedicalDbContext RepoMedicalDBContext { get; }
        IRepositoryBase<TblUser> UserRepository { get; }
        Task Commit();
    }
    
}
