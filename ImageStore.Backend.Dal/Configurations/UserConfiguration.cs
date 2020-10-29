using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ImageStore.Backend.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageStore.Backend.Dal.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
        }
    }
}
