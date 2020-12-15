using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using UploadImage.Models;

namespace UploadImage.Data
{
    public class ImageDbContext : IdentityDbContext
    {
        public ImageDbContext()
        {
        }
        public ImageDbContext(DbContextOptions<ImageDbContext> options)
            : base(options)
        {
        }

        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model.Entity<Image>()
                .HasOne(x => x.User)
                .WithMany(u => u.Images)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(model);
        }
    }
}
