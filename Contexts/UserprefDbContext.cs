using CS2Fixes_ASP_DOTNET_Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace CS2Fixes_ASP_DOTNET_Core.Contexts;

public class UserprefDbContext : DbContext
{
    public UserprefDbContext(DbContextOptions<UserprefDbContext> options) : base(options) { }
    public DbSet<Userpref> Userprefs { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Entities.Userpref>()
            .HasKey(up => new { up.Steamid, up.Key });
        base.OnModelCreating(modelBuilder);
    }
}
