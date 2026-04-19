using EfCoreCodeFirst.DAL;
using EfCoreCodeFirst.DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace EfCoreCodeFirst;

public class Demo9AdvancedQueries
{
    public static void Run()
    {
        Console.WriteLine("=== Проекции и группировки ===");

        using var db = new DbContextFactory().CreateDbContext(["log"]);

        // Проекция в DTO
        var productDtos = db.Products
            .Include(p=>p.User)
            .Select(p => new ProductDto
            {
                Title = p.Title,
                Price = p.Price,
                OwnerName = p.User.Name
            })
            .ToList();

        Console.WriteLine("Проекция в ProductDto:");
        foreach (var d in productDtos)
            Console.WriteLine($"  {d.Title} — {d.Price:C} (владелец: {d.OwnerName})");

        // Группировка с агрегацией
        var userTotals = db.Products
            .GroupBy(p => p.UserId)
            .Select(g => new
            {
                UserId = g.Key,
                TotalPrice = g.Sum(p => p.Price),
                ProductCount = g.Count()
            })
            .ToList();

        Console.WriteLine("\nГруппировка по UserId:");
        foreach (var g in userTotals)
            Console.WriteLine($"  UserId={g.UserId}: всего {g.ProductCount} товаров на сумму {g.TotalPrice:C}");

        // Join в синтаксисе query expression
        var result = (
            from p in db.Products
            join u in db.Users on p.UserId equals u.Id
            group p by new { u.Id, u.Name } into g
            select new
            {
                g.Key.Name,
                Total = g.Sum(x => x.Price),
                Count = g.Count()
            }).ToList();

        Console.WriteLine("\nСумма продуктов по каждому пользователю (join):");
        foreach (var r in result)
            Console.WriteLine($"  {r.Name}: {r.Count} товаров, сумма {r.Total:C}");
    }
}