using System;
using Files.Management.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Files.Management.Persistence.Map
{
    public class EditedImageMap : IEntityTypeConfiguration<EditedImage>
    {
        public void Configure(EntityTypeBuilder<EditedImage> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id);
            builder.Property(x => x.Width);
            builder.Property(x => x.Height);
            builder.Property(x => x.ImageId);
            builder.Property(x => x.Operation);
            builder.Property(x => x.Path);
        }
    }
}
