namespace Lol_Champion_Mastery_Booster.Data
{
    using Lol_Champion_Mastery_Booster.Models;
    using Microsoft.EntityFrameworkCore;

    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<Profile> Profiles => Set<Profile>();
    }

}
