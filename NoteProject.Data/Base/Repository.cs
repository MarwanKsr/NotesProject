using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NoteProject.Core.Base;
using System.Linq.Expressions;

namespace NoteProject.Data.Base;

public class Repository<TEntity, TDbContext> : IRepository<TEntity, TDbContext>
    where TEntity : BaseEntity
    where TDbContext : DbContext
{
    #region Ctor

    private readonly TDbContext _context;
    public Repository(TDbContext context)
    {
        _context = context;
    }

    #endregion

    #region Basics

    public void Attach(TEntity entity, EntityState? state = null)
    {
        _context.Set<TEntity>().Attach(entity);

        if (state.HasValue)
            _context.Entry(entity).State = state.Value;
    }

    #endregion

    #region Query & List
    public IQueryable<TEntity> QueryAll(params Expression<Func<TEntity, object>>[] eagerProperties)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();

        query = eagerProperties.Aggregate(query, (current, e) => current.Include(e));

        return query;
    }
    public IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties)
    {
        return QueryAll(eagerProperties).Where(predicate);
    }

    public List<TEntity> ListAll(params Expression<Func<TEntity, object>>[] eagerProperties)
    {
        return QueryAll(eagerProperties).ToList();
    }
    public async Task<List<TEntity>> ListAllAsync(params Expression<Func<TEntity, object>>[] eagerProperties)
    {
        return await QueryAll(eagerProperties).ToListAsync();
    }

    public List<TEntity> List(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties)
    {
        return Query(predicate, eagerProperties).ToList();
    }
    public async Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties)
    {
        return await Query(predicate, eagerProperties).ToListAsync();
    }

    #endregion

    #region Find & First Or Default
    public TEntity Find(params object[] pk)
    {
        try
        {
            var entity = _context.Set<TEntity>().Find(pk);

            return entity;
        }
        catch (Exception)
        {

            return null;
        }

    }
    public async Task<TEntity> FindAsync(params object[] pk)
    {
        var entity = await _context.Set<TEntity>().FindAsync(pk);

        return entity;
    }

    public TEntity FindDetached(long id)
    {
        try
        {
            var entity = _context.Set<TEntity>().AsNoTracking().FirstOrDefault(t => t.Id == id);

            return entity;
        }
        catch (Exception)
        {

            return null;
        }

    }
    public async Task<TEntity> FindDetachedAsync(long id)
    {
        try
        {
            var entity = await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(t => t.Id == id);

            return entity;
        }
        catch (Exception)
        {

            return null;
        }

    }


    public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();
        query = eagerProperties.Aggregate(query, (current, e) => current.Include(e));

        return query.Where(predicate).FirstOrDefault();
    }
    public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();
        query = eagerProperties.Aggregate(query, (current, e) => current.Include(e));

        return await query.Where(predicate).FirstOrDefaultAsync();
    }

    public TEntity LastOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();
        query = eagerProperties.Aggregate(query, (current, e) => current.Include(e));

        return query.Where(predicate).LastOrDefault();
    }
    public async Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();
        query = eagerProperties.Aggregate(query, (current, e) => current.Include(e));

        return await query.Where(predicate).LastOrDefaultAsync();
    }

    public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();
        query = eagerProperties.Aggregate(query, (current, e) => current.Include(e));

        return query.Where(predicate).SingleOrDefault();
    }
    public async Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties)
    {
        IQueryable<TEntity> query = _context.Set<TEntity>().AsQueryable();
        query = eagerProperties.Aggregate(query, (current, e) => current.Include(e));

        return await query.Where(predicate).SingleOrDefaultAsync();
    }


    #endregion

    #region All & Any 
    public bool All(Expression<Func<TEntity, bool>> predicate)
    {
        return QueryAll().All(predicate);
    }
    public async Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await QueryAll().AllAsync(predicate);
    }

    public bool Any(Expression<Func<TEntity, bool>> predicate)
    {
        return QueryAll().Any(predicate);
    }

    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate)
    {
        return await QueryAll().AnyAsync(predicate);
    }
    #endregion

    #region Remove
    public void Remove(long pk)
    {
        var entity = Find(pk);

        Attach(entity);

        _context.Set<TEntity>().Remove(entity);
    }
    public void Remove(TEntity entity)
    {
        Attach(entity);

        _context.Set<TEntity>().Remove(entity);
    }

    public void RemoveRange(IEnumerable<long> pks)
    {
        foreach (var pk in pks)
            Remove(pk);
    }
    public void RemoveRange(IEnumerable<TEntity> entities)
    {
        foreach (var entity in entities)
            Remove(entity);
    }

    #endregion

    #region Remove And Save
    public int RemoveAndSave(long pk)
    {
        Remove(pk);

        return Save();
    }
    public int RemoveAndSave(TEntity entity)
    {
        Remove(entity);

        return Save();
    }

    public async Task<int> RemoveAndSaveAsync(long pk)
    {
        Remove(pk);

        return await SaveAsyc();
    }
    public async Task<int> RemoveAndSaveAsync(TEntity entity)
    {
        Remove(entity);

        return await SaveAsyc();
    }

    public int RemoveRangeAndSave(IEnumerable<long> pks)
    {
        RemoveRange(pks);

        return Save();
    }
    public int RemoveRangeAndSave(IEnumerable<TEntity> entities)
    {
        RemoveRange(entities);

        return Save();
    }

    public async Task<int> RemoveRangeAndSaveAsync(IEnumerable<long> pks)
    {
        RemoveRange(pks);

        return await SaveAsyc();
    }
    public async Task<int> RemoveRangeAndSaveAsync(IEnumerable<TEntity> entities)
    {
        RemoveRange(entities);

        return await SaveAsyc();
    }

    #endregion

    #region Modify
    public void Modify(TEntity entity)
    {
        Attach(entity, EntityState.Modified);
    }
    public void ModifyRange(List<TEntity> list)
    {
        foreach (var entity in list)
            Attach(entity, EntityState.Modified);
    }

    #endregion

    #region Modify And Save

    public int ModifyAndSave(TEntity entity)
    {
        Modify(entity);

        return Save();
    }
    public int ModifyRangeAndSave(List<TEntity> list)
    {
        ModifyRange(list);

        return Save();
    }

    public async Task<int> ModifyAndSaveAsync(TEntity entity)
    {
        Modify(entity);

        return await SaveAsyc();
    }
    public async Task<int> ModifyRangeAndSaveAsync(List<TEntity> list)
    {
        ModifyRange(list);

        return await SaveAsyc();
    }

    #endregion

    #region Add & Insert
    public void Add(TEntity entity)
    {
        Attach(entity, EntityState.Added);

        _context.Set<TEntity>().Add(entity);
    }

    public void AddRange(List<TEntity> entites)
    {
        foreach (var entity in entites)
        {
            Attach(entity, EntityState.Added);
            _context.Set<TEntity>().Add(entity);
        }
    }

    public TEntity AddEntity(TEntity entity)
    {
        Attach(entity, EntityState.Added);

        _context.Set<TEntity>().Add(entity);

        return entity;
    }

    #endregion

    #region Add & Save

    public void AddAndSave(TEntity entity)
    {
        Add(entity);
        Save();
    }

    public async Task AddAndSaveAsync(TEntity entity)
    {
        Add(entity);

        await SaveAsyc();
    }

    public int AddRangeAndSave(List<TEntity> entites)
    {
        AddRange(entites);

        return Save();
    }
    public async Task<int> AddRangeAndSaveAsync(List<TEntity> entites)
    {
        AddRange(entites);

        return await SaveAsyc();
    }

    #endregion

    #region Count
    public int Count(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties)
    {
        return Query(predicate, eagerProperties).Count();
    }

    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties)
    {
        return await Query(predicate, eagerProperties).CountAsync();
    }

    public int CountAll(params Expression<Func<TEntity, object>>[] eagerProperties)
    {
        return QueryAll(eagerProperties).Count();
    }

    public async Task<int> CountAllAsync(params Expression<Func<TEntity, object>>[] eagerProperties)
    {
        return await QueryAll(eagerProperties).CountAsync();
    }
    #endregion

    #region Transaction

    public IDbContextTransaction BeginTransaction()
    {
        return _context.Database.BeginTransaction();
    }

    #endregion

    #region Save

    public int Save()
    {
        return _context.SaveChanges();
    }

    public async Task<int> SaveAsyc()
    {
        return await _context.SaveChangesAsync();
    }

    #endregion
}
