using EfCoreCodeFirst.DAL;
using Microsoft.EntityFrameworkCore;

namespace EfCoreCodeFirst;

public class Demo7RawSql
{
    public static void Run()
    {
        Console.WriteLine("=== Raw SQL ===");

        using var db = new DbContextFactory().CreateDbContext(["log"]);

        // 1. FromSqlRaw — параметризованный SQL, возвращающий сущности
        string search = "%шка%";
        var products = db.Products
            .FromSqlRaw("SELECT * FROM table_products WHERE \"Title\" LIKE {0}", search)
            .Include(p => p.User)
            .ToList();

        Console.WriteLine($"FromSqlRaw: найдено {products.Count} по шаблону '{search}'");
        foreach (var p in products)
            Console.WriteLine($"  {p.Title} — владелец {p.User?.Name}");

        // 2. FromSqlInterpolated — безопасная интерполяция (параметры идут как SQL-параметры, не склеиваются)
        var minPrice = 50000m;
        var expensive = db.Products
            .FromSqlInterpolated($"SELECT * FROM table_products WHERE \"Price\" > {minPrice}")
            .ToList();
        Console.WriteLine($"FromSqlInterpolated: дороже {minPrice:C} — {expensive.Count} шт.");

        // 3. ExecuteSqlRaw — команды UPDATE/DELETE/INSERT без маппинга в сущности
        int rowsAffected = db.Database.ExecuteSqlRaw(
            "UPDATE table_users SET \"Name\" = {0} WHERE \"Email\" = {1}",
            "Мария Тифонова", "maria@example.com");
        Console.WriteLine($"ExecuteSqlRaw: обновлено строк — {rowsAffected}");

        // ВАЖНО: параметры защищают от SQL-инъекций.
        // Никогда не склеивайте строки SQL с пользовательским вводом через "+" или $"...".
    }
}