using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NoteProject.Core.Base;
using System.Linq.Expressions;

namespace NoteProject.Data.Base;

public interface IRepository<TEntity, TDbContext>
    where TEntity : BaseEntity
    where TDbContext : DbContext
{
    #region Basics
    void Attach(TEntity entity, EntityState? state = null);
    #endregion

    #region Query & List
    IQueryable<TEntity> QueryAll(params Expression<Func<TEntity, object>>[] eagerProperties);
    IQueryable<TEntity> Query(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties);

    List<TEntity> ListAll(params Expression<Func<TEntity, object>>[] eagerProperties);
    Task<List<TEntity>> ListAllAsync(params Expression<Func<TEntity, object>>[] eagerProperties);

    List<TEntity> List(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties);
    Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties);

    #endregion

    #region Find & First Or Default
    TEntity Find(params object[] pk);
    Task<TEntity> FindAsync(params object[] pk);

    TEntity FindDetached(long id);
    Task<TEntity> FindDetachedAsync(long id);

    TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties);
    Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties);

    TEntity LastOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties);
    Task<TEntity> LastOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties);

    TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties);
    Task<TEntity> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties);

    #endregion

    #region All & Any 
    bool All(Expression<Func<TEntity, bool>> predicate);
    Task<bool> AllAsync(Expression<Func<TEntity, bool>> predicate);

    bool Any(Expression<Func<TEntity, bool>> predicate);
    Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);

    #endregion

    #region Remove
    void Remove(long pk);
    void Remove(TEntity entity);

    void RemoveRange(IEnumerable<long> pks);
    void RemoveRange(IEnumerable<TEntity> entities);
    #endregion

    #region Remove And Save
    int RemoveAndSave(long pk);
    int RemoveAndSave(TEntity entity);

    Task<int> RemoveAndSaveAsync(long pk);
    Task<int> RemoveAndSaveAsync(TEntity entity);

    int RemoveRangeAndSave(IEnumerable<long> pks);
    int RemoveRangeAndSave(IEnumerable<TEntity> entities);

    Task<int> RemoveRangeAndSaveAsync(IEnumerable<long> pks);
    Task<int> RemoveRangeAndSaveAsync(IEnumerable<TEntity> entities);
    #endregion

    #region Modify
    void Modify(TEntity entity);
    void ModifyRange(List<TEntity> list);
    #endregion

    #region Modify And Save

    int ModifyAndSave(TEntity entity);
    int ModifyRangeAndSave(List<TEntity> list);

    Task<int> ModifyAndSaveAsync(TEntity entity);
    Task<int> ModifyRangeAndSaveAsync(List<TEntity> list);
    #endregion

    #region Add & Insert
    void Add(TEntity entity);

    void AddRange(List<TEntity> entites);

    TEntity AddEntity(TEntity entity);

    #endregion

    #region Add & Save

    void AddAndSave(TEntity entity);
    Task AddAndSaveAsync(TEntity entity);

    int AddRangeAndSave(List<TEntity> entites);
    Task<int> AddRangeAndSaveAsync(List<TEntity> entites);

    #endregion

    #region Count
    int Count(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties);
    Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] eagerProperties);

    int CountAll(params Expression<Func<TEntity, object>>[] eagerProperties);
    Task<int> CountAllAsync(params Expression<Func<TEntity, object>>[] eagerProperties);

    #endregion

    #region Transaction

    IDbContextTransaction BeginTransaction();

    #endregion

    #region Save

    int Save();
    Task<int> SaveAsyc();

    #endregion
}
