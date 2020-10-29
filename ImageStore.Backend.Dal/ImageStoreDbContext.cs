using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ImageStore.Backend.Dal.Configurations;
using ImageStore.Backend.Dal.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImageStore.Backend.Dal
{
    public class ImageStoreDbContext : IdentityDbContext<User, IdentityRole<int>, int>
    {
        public DbSet<Image> Images { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public ImageStoreDbContext(DbContextOptions<ImageStoreDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfiguration(new UserConfiguration());
            builder.ApplyConfiguration(new ImageConfiguration());
            builder.ApplyConfiguration(new CommentConfiguration());
        }
    }
}
