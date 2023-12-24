using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using NoteProject.Core.Extensions;
using NoteProject.Data.Extensions;
using NoteProject.Core.Domain.Identity;

namespace NoteProject.Data.Identity;

public class IdentityModelContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    private readonly IHttpContextAccessor _context;

    public IdentityModelContext() { }
    public IdentityModelContext(DbContextOptions<IdentityModelContext> options, IHttpContextAccessor context)
        : base(options)
    {
        _context = context;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Add IsDeleted Query filter
        builder.ConfigureSoftDeleteEntitiesFilter();
    }

    public override int SaveChanges()
    {
        var userName = _context.HttpContext?.User?.GetUserId() ?? "";

        ChangeTracker.AlterAuditableEntities(userName);
        ChangeTracker.AlterSoftDeleteEntities(userName);

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var userName = _context.HttpContext?.User?.GetUserId() ?? "";

        ChangeTracker.AlterAuditableEntities(userName);
        ChangeTracker.AlterSoftDeleteEntities(userName);

        return await base.SaveChangesAsync(cancellationToken);
    }
}
