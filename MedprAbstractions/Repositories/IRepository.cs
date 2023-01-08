using MedprCore;
using MedprDB.Entities;
using System.Linq.Expressions;

namespace MedprAbstractions.Repositories
{
    public interface IRepository<T> where T : IBaseEntity
    {
        //READ
        Task<T?> GetByIdAsync(Guid id);

        Task<IEnumerable<T>> GetAllAsync();

        IQueryable<T> Get();

        IQueryable<T> FindBy(Expression<Func<T, bool>> searchExpression,
            params Expression<Func<T, object>>[] includes);

        //CREATE
        Task AddAsync(T entity);

        Task AddRangeAsync(IEnumerable<T> entities);

        //UPDATE
        void Update(T entity);

        Task PatchAsync(Guid id, List<PatchModel> patchData);

        //DELETE
        void Remove(T entity);
    }
}