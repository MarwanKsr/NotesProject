using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NoteProject.Core.Domain.Notes;

namespace NoteProject.Data.Data.EntitesConfigurations;

public class NoteEntityTypeConfiguration : IEntityTypeConfiguration<Note>
{
    public void Configure(EntityTypeBuilder<Note> builder)
    {
        builder.HasOne(e => e.Image).WithMany().OnDelete(DeleteBehavior.NoAction);
        builder.Navigation(e => e.Image).AutoInclude();
    }
}
