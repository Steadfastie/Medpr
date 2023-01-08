using MedprAbstractions.Repositories;
using MedprCore;
using MedprDB;
using MedprDB.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MedprRepositories;

public class Repository<T> : IRepository<T> where T : class, IBaseEntity
{
    protected readonly MedprDBContext _database;
    protected readonly DbSet<T> _dbSet;

    public Repository(MedprDBContext database)
    {
        _database = database;
        _dbSet = database.Set<T>();
    }

    public virtual async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
    }

    public virtual async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await _dbSet.AddRangeAsync(entities);
    }

    public virtual async Task<T?> GetByIdAsync(Guid id)
    {
        return await _dbSet.AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id.Equals(id));
    }

    public virtual async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public virtual IQueryable<T> Get()
    {
        return _dbSet;
    }

    public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> searchExpression,
        params Expression<Func<T, object>>[] includes)
    {
        var result = _dbSet.Where(searchExpression);
        if (includes.Any())
        {
            result = includes.Aggregate(result, (current, include) =>
                current.Include(include));
        }
        return result;
    }

    public virtual void Update(T entity)
    {
        _dbSet.Update(entity);
    }

    public virtual async Task PatchAsync(Guid id, List<PatchModel> patchData)
    {
        var model = await _dbSet.FirstOrDefaultAsync(entity => entity.Id.Equals(id));

        var nameValuePropertiesPairs = patchData
            .ToDictionary(
                patchModel => patchModel.PropertyName,
                patchModel => patchModel.PropertyValue);

        var dbEntityEntry = _database.Entry(model);
        dbEntityEntry.CurrentValues.SetValues(nameValuePropertiesPairs);
        dbEntityEntry.State = EntityState.Modified;
    }

    public virtual void Remove(T entity)
    {
        _dbSet.Remove(entity);
    }
}