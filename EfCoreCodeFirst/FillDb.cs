using EfCoreCodeFirst.DAL;
using EfCoreCodeFirst.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace EfCoreCodeFirst;

public class FillDb
{
    public static void Run()
    {
        Console.WriteLine("=== Наполнение БД тестовыми данными ===");
        
        using var db = new DbContextFactory().CreateDbContext();
        
        db.Database.Migrate();
        
        db.Products.RemoveRange(db.Products);
        db.Users.RemoveRange(db.Users);
        db.Categories.RemoveRange(db.Categories);
        
        db.SaveChanges();
        
        var laptops    = new Category { Name = "Ноутбуки" };
        var phones     = new Category { Name = "Смартфоны" };
        var accessories = new Category { Name = "Аксессуары" };
        db.Categories.AddRange(laptops, phones, accessories);
        
        var anna = new User { Name = "Анна Иванова", Email = "anna@test.com", CreatedAt = DateTime.UtcNow };
        var boris = new User { Name = "Борис Смирнов", Email = "boris@test.com", CreatedAt = DateTime.UtcNow };
        var alex = new User { Name = "Александр Ким", Email = "alex@test.com", CreatedAt = DateTime.UtcNow };
        db.Users.AddRange(anna, boris, alex);
        
        db.Products.AddRange(
            new Product { Title = "MacBook Pro",    Price = 180000, User = anna,  Category = laptops },
            new Product { Title = "Lenovo ThinkPad", Price = 95000,  User = anna,  Category = laptops },
            new Product { Title = "iPhone 15",      Price = 90000,  User = boris, Category = phones },
            new Product { Title = "Samsung S24",    Price = 75000,  User = boris, Category = phones },
            new Product { Title = "Чехол",           Price = 800,    User = alex,  Category = accessories },
            new Product { Title = "Мышка",           Price = 1500,   User = alex,  Category = accessories }
        );
        
        db.SaveChanges();
        Console.WriteLine($"Добавлено: пользователей={db.Users.Count()}, продуктов={db.Products.Count()}, категорий={db.Categories.Count()}");
        
    }
}