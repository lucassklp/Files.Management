using System;
using Files.Management.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Files.Management.Persistence.Map
{
    public class ImageMap : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.Property(x => x.Width);
            builder.Property(x => x.Height);

            builder.HasMany(x => x.EditedImages)
                .WithOne(x => x.Image)
                .HasForeignKey(x => x.ImageId);
        }
    }
}
