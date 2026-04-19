using EfCoreCodeFirst.DAL;

namespace EfCoreCodeFirst;

public class Demo5Linq
{
    public static void Run()
    {
        Console.WriteLine("=== LINQ: базовые запросы ===");
        
        using var db = new DbContextFactory().CreateDbContext(["log"]);
        //
        //
        // var products = db.Products
        //     .Where(p => p.Price > 50000)
        //     .OrderByDescending(p => p.Price)
        //     .Select(p => new { p.Title });
        //
        // foreach (var product in products)
        // {
        //     Console.WriteLine(product.Title);
        // }
        
        bool hasBigger = db.Products.Any(p => p.Price > 200000);
        
        Console.WriteLine($"Есть товары дороже 100000: {hasBigger}");



    }
}