using CS2Fixes_ASPCore.Entities;
using Microsoft.EntityFrameworkCore;

namespace CS2Fixes_ASPCore.Contexts;

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
