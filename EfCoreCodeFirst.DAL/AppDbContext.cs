using EfCoreCodeFirst.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace EfCoreCodeFirst.DAL;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Category> Categories{ get; set; }


    public AppDbContext(DbContextOptions options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().ToTable("table_users");
        modelBuilder.Entity<Product>().ToTable("table_products");
        modelBuilder.Entity<Category>().ToTable("table_categories");
        
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        
        modelBuilder.Entity<Product>()
            .HasOne(p => p.User)
            .WithMany(u => u.Products)
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.Cascade);
        
        modelBuilder.Entity<Product>()
            .HasOne(c => c.Category)
            .WithMany(c => c.Products)
            .HasForeignKey(c => c.CategoryId)
            .OnDelete(DeleteBehavior.SetNull);
        
        modelBuilder.Entity<Product>()
            .Property(p => p.Id)
            .HasPrecision(18, 2);
    }
}