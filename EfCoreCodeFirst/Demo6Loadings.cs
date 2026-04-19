using EfCoreCodeFirst.DAL;
using Microsoft.EntityFrameworkCore;

namespace EfCoreCodeFirst;

public class Demo6Loadings
{
    public static void Run()
    {
        //EagerLoading();
        ExplicitLoading();
        //LazyLoadingNote();
    }
    
    
    private static void EagerLoading()
    {
        Console.WriteLine("\n-- 1. Eager Loading (Include / ThenInclude) --");
        using var db = new DbContextFactory().CreateDbContext(["log"]);

        var users = db.Users
            .Include(u => u.Products)
            .ThenInclude(p => p.Category)
            .ToList();

        foreach (var u in users)
        {
            Console.WriteLine($"{u.Name}:");
            foreach (var p in u.Products)
                Console.WriteLine($"    - {p.Title} [{p.Category?.Name ?? "без категории"}]");
        }
    }
    

    private static void ExplicitLoading()
    {
        Console.WriteLine("\n-- 2. Explicit Loading (Entry.Collection.Load) --");
        using var db = new DbContextFactory().CreateDbContext(["log"]);

        var user = db.Users.First();
        Console.WriteLine($"Пользователь: {user.Name} (продукты ещё не загружены {user.Products?.Count }) ");

        // Явный запрос продуктов конкретного пользователя
        db.Entry(user).Collection(u => u.Products).Load();

        Console.WriteLine($"После Load() — продуктов: {user.Products.Count}");
        foreach (var p in user.Products)
            Console.WriteLine($"    - {p.Title}");
    }

    private static void LazyLoadingNote()
    {
        Console.WriteLine("\n-- 3. Lazy Loading --");
        Console.WriteLine("Требует: пакет Microsoft.EntityFrameworkCore.Proxies,");
        Console.WriteLine("вызов UseLazyLoadingProxies() в OnConfiguring и virtual у нав. свойств.");
        Console.WriteLine("Тогда обращение к user.Products автоматически выполняет SELECT.");
        Console.WriteLine("ОСТОРОЖНО: скрывает проблему N+1 — в цикле легко получить сотни запросов.");
    }
}