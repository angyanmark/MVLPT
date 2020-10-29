using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ImageStore.Backend.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageStore.Backend.Dal.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.ToTable(nameof(Image));

            builder.HasKey(i => i.Id);

            builder.Property(i => i.FileName).IsRequired();

            builder.Property(i => i.OriginalFileName).IsRequired();

            builder.HasOne(i => i.Uploader)
                .WithMany(u => u.Images);
        }
    }
}
