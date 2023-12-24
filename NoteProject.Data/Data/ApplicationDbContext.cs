using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NoteProject.Core.Domain.Medias;
using NoteProject.Core.Domain.Notes;
using NoteProject.Data.Data.EntitesConfigurations;
using NoteProject.Data.Extensions;
using NoteProject.Core.Extensions;

namespace NoteProject.Data.Data;

public class ApplicationDbContext : DbContext
{
    private readonly IHttpContextAccessor _context;

    public ApplicationDbContext() { }
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IHttpContextAccessor context)
        : base(options)
    {
        _context = context;
    }

    public DbSet<Media> Medias { get; set; }
    public DbSet<Note> Notes { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.ApplyConfiguration(new NoteEntityTypeConfiguration());

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
