using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using NoteProject.Core.Base;

namespace NoteProject.Data.Extensions;

public static class ChangeTrackerExtensions
{
    public static void AlterAuditableEntities(this ChangeTracker changeTracker, string currentUserName)
    {
        var changeSet = changeTracker.Entries<IAuditable>();
        var now = DateTime.UtcNow;

        foreach (var entry in changeSet)
        {
            if (entry.State is EntityState.Added)
            {
                entry.Entity.CreatedBy = currentUserName;
                entry.Entity.CreatedAt = now;
            }
            else if (entry.State is EntityState.Modified)
            {
                entry.Property(x => x.CreatedBy).IsModified = false;
                entry.Property(x => x.CreatedAt).IsModified = false;
            }
            else continue;

            entry.Entity.AuditModify(currentUserName);
        }

    }

    public static void AlterSoftDeleteEntities(this ChangeTracker changeTracker, string currentUserName)
    {
        var softDeleteEntries = changeTracker.Entries<ISoftDeleteMarker>()
            .Where(e => e.State is EntityState.Deleted)
            .Select(e => e.Entity)
            .ToList();

        if (!softDeleteEntries.Any())
            return;

        changeTracker.Clear();

        foreach (var entry in softDeleteEntries)
        {
            entry.SoftDelete(currentUserName);

            changeTracker.Context.Update(entry);
        }
    }
}
