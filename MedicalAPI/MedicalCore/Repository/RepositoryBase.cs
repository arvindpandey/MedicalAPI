using MedicalAPI.MedicalEntity;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedicalAPI.MedicalCore.Repository
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected MedicalDbContext RepositoryContext { get; set; }
        public RepositoryBase(MedicalDbContext repositoryContext)
        {
            RepositoryContext = repositoryContext;
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await RepositoryContext.Set<T>().ToListAsync();
        }
        public async Task<T> GetById<Tkey>(Tkey id)
        {
            return await RepositoryContext.Set<T>().FindAsync(id);
        }
        public async Task<T> Add(T obj)
        {
            await RepositoryContext.Set<T>().AddAsync(obj);
            return obj;
        }
        public async Task<T> Update(T obj)
        {
            RepositoryContext.Set<T>().Update(obj);
            return obj;
        }
        public void Delete(T obj)
        {
            RepositoryContext.Set<T>().Remove(obj);
        }
        public IQueryable<T> AsQueryable()
        {
            return RepositoryContext.Set<T>().AsQueryable();
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> expression)
        {
            return await RepositoryContext.Set<T>().Where(expression).FirstOrDefaultAsync();
        }

        public async Task<bool> Any(Expression<Func<T, bool>> expression)
        {
            return await RepositoryContext.Set<T>().Where(expression).AnyAsync();
        }
        public void Detach(T entity)
        {
            RepositoryContext.Entry(entity).State = EntityState.Detached;
        }
    }
    public interface IRepositoryBase<T>
    {
        Task<IEnumerable<T>> GetAll();
        IQueryable<T> AsQueryable();
        Task<T> GetById<Tkey>(Tkey id);
        Task<T> Add(T obj);
        Task<T> Update(T obj);
        void Delete(T obj);
        Task<T> FirstOrDefault(Expression<Func<T, bool>> expression);
        Task<bool> Any(Expression<Func<T, bool>> expression);
        void Detach(T entity);
    }
}
