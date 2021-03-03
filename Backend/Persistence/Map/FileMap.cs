using Files.Management.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Files.Management.Persistence.Map
{
    public class FileMap : IEntityTypeConfiguration<File>
    {
        public void Configure(EntityTypeBuilder<File> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Date).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(150).IsRequired();
            builder.Property(x => x.Path).IsRequired();
            builder.Property(x => x.IsPublic).IsRequired();
            builder.Property(x => x.UID).IsRequired();
            builder.Property(x => x.Filename).HasMaxLength(150).IsRequired();

            builder.HasIndex(x => x.UID);
            builder.HasIndex(x => x.Date);

            builder.HasOne(x => x.User)
                .WithMany(x => x.Files)
                .HasForeignKey(x => x.UserId);
        }
    }
}
