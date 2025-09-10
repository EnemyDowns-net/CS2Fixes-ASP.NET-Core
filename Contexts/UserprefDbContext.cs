using CS2Fixes_ASP_DOTNET_Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CS2Fixes_ASP_DOTNET_Core.Contexts;

public class UserprefDbContext : DbContext
{
    public UserprefDbContext(DbContextOptions<UserprefDbContext> options) : base(options) { }
    public DbSet<Userpref> Userprefs { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Userpref>()
            .Property(e => e.Key)
            .HasMaxLength(64);

        modelBuilder.Entity<Userpref>()
            .Property(e => e.Value)
            .HasMaxLength(64);

        modelBuilder.Entity<Userpref>()
            .HasKey(up => new { up.Steamid, up.Key });
    }
}
