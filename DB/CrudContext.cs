using Microsoft.EntityFrameworkCore;

namespace DB;

public class CrudContext : DbContext
{
    public CrudContext(DbContextOptions<CrudContext> options) : base(options)
    {
        
    }
    
    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //para no modificar al pedo la tabla en bd
        modelBuilder.Entity<User>().ToTable("User");
    }
}