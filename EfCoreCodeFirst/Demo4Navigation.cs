using EfCoreCodeFirst.DAL;
using EfCoreCodeFirst.DAL.Models;

namespace EfCoreCodeFirst;

public class Demo4
{
    public static void Run()
    {
        Console.WriteLine("=== Навигационные свойства ===");
        
        using var db = new DbContextFactory().CreateDbContext(["log"]);

        var user = db.Users.First();
        var product = new Product
        {
            Title = "Клавиатура",
            Price = 3500,
            User = user   // EF сам выставит UserId = user.Id
        };
        db.Products.Add(product);
        db.SaveChanges();
        Console.WriteLine($"Создан продукт '{product.Title}' c UserId={product.UserId}");
        
        

    }
}