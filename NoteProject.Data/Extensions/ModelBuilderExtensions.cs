using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using NoteProject.Core.Base;
using System.Linq.Expressions;

namespace NoteProject.Data.Extensions;

public static class ModelBuilderExtensions
{
    public static ModelBuilder EntitiesOfType<T>(this ModelBuilder modelBuilder,
        Action<EntityTypeBuilder> buildAction) where T : class
        => modelBuilder.EntitiesOfType(typeof(T), buildAction);

    public static ModelBuilder EntitiesOfType(this ModelBuilder modelBuilder, Type type,
        Action<EntityTypeBuilder> buildAction)
    {
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            if (type.IsAssignableFrom(entityType.ClrType))
                buildAction(modelBuilder.Entity(entityType.ClrType));

        return modelBuilder;
    }

    public static void ConfigureSoftDeleteEntitiesFilter(this ModelBuilder modelBuilder)
    {
        // Add IsDeleted Query filter
        modelBuilder.EntitiesOfType<ISoftDeleteMarker>(builder =>
        {
            var param = Expression.Parameter(builder.Metadata.ClrType, "p");
            var body = Expression.Equal(Expression.Property(param, nameof(ISoftDeleteMarker.IsDeleted)), Expression.Constant(false));
            var lambda = Expression.Lambda(body, param);

            builder.HasQueryFilter(lambda);
        });
    }
}
