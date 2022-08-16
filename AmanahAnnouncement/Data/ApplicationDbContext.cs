using AmanahAnnouncement.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AmanahAnnouncement.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Announcment> Announcments { get; set; }
        public DbSet<FileModel> FileModel { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Users> Users { get; }
    }
}
