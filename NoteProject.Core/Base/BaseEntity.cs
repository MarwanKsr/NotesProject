
namespace NoteProject.Core.Base;

public interface IBaseEntity<T> where T : IComparable<T>
{
    public T Id { get; set; }
}

public abstract class BaseEntity : IBaseEntity<long>
{
    public long Id { get; set; }
}

public interface IAuditable
{
    string CreatedBy { get; set; }
    DateTime CreatedAt { get; set; }
    string ModifiedBy { get; }
    DateTime? ModifiedAt { get; }
    void AuditModify(string modifiedBy);
}

public interface ISoftDeleteMarker
{
    public bool IsDeleted { get; }
    public DateTime DeletedAt { get; }
    public string DeletedBy { get; }

    void SoftDelete(string deletedBy);
}

public abstract class AuditableBaseEntity : BaseEntity, IAuditable
{
    public string CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; }

    public string ModifiedBy { get; private set; }

    public DateTime? ModifiedAt { get; private set; }
    public void AuditModify(string modifiedBy)
    {
        ModifiedBy = modifiedBy;
        ModifiedAt = DateTime.UtcNow;
    }
}

public abstract class SoftDeleteAuditableBaseEntity : AuditableBaseEntity, ISoftDeleteMarker
{
    public bool IsDeleted { get; private set; }

    public DateTime DeletedAt { get; private set; }

    public string DeletedBy { get; private set; } = "";

    public void SoftDelete(string deletedBy)
    {
        DeletedBy = deletedBy;
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}
