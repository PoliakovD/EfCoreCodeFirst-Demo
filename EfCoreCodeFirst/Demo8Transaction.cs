using EfCoreCodeFirst.DAL;
using EfCoreCodeFirst.DAL.Models;

namespace EfCoreCodeFirst;

public class Demo8Transaction
{
     public static void Run()
    {
        Console.WriteLine("=== Транзакции ===");

        ImplicitTransaction();
        ExplicitTransaction();
        ExplicitTransactionWithError();
    }

    // Все изменения одного SaveChanges() автоматически оборачиваются в транзакцию.
    private static void ImplicitTransaction()
    {
        Console.WriteLine("\n-- Неявная транзакция (один SaveChanges) --");
        using var db = new DbContextFactory().CreateDbContext(["log"]);

        var user = new User
        {
            Name = "Транзакционный Пользователь",
            Email = $"tr_{Guid.NewGuid():N}@test.com",
            CreatedAt = DateTime.UtcNow
        };
        db.Users.Add(user);
        db.Products.Add(new Product { Title = "Товар A", Price = 500, User = user });
        db.Products.Add(new Product { Title = "Товар B", Price = 700, User = user });

        db.SaveChanges();   // всё либо сохранится целиком, либо не сохранится
        Console.WriteLine("  Пользователь и два товара сохранены в одной транзакции");
    }

    // Явная транзакция — когда нужно несколько SaveChanges() или смешивать с Raw SQL.
    private static void ExplicitTransaction()
    {
        Console.WriteLine("\n-- Явная транзакция (BeginTransaction + Commit) --");
        using var db = new DbContextFactory().CreateDbContext(["log"]);

        using var transaction = db.Database.BeginTransaction();
        try
        {
            var user = new User
            {
                Name = "Явная транзакция",
                Email = $"ex_{Guid.NewGuid():N}@test.com",
                CreatedAt = DateTime.UtcNow
            };
            db.Users.Add(user);
            db.SaveChanges();

            db.Products.Add(new Product
            {
                Title = "Продукт в транзакции",
                Price = 100,
                UserId = user.Id
            });
            db.SaveChanges();

            transaction.Commit();
            Console.WriteLine("  Транзакция успешно зафиксирована");
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine($"  Ошибка: {ex.Message}. Выполнен откат.");
        }
    }

    // Иллюстрация отката: пытаемся вставить пользователя с уже существующим email — сработает уникальный индекс.
    private static void ExplicitTransactionWithError()
    {
        Console.WriteLine("\n-- Транзакция с ошибкой и откатом --");
        using var db = new DbContextFactory().CreateDbContext(["log"]);

        using var transaction = db.Database.BeginTransaction();
        try
        {
            db.Users.Add(new User
            {
                Name = "Первый",
                Email = "duplicate@test.com",
                CreatedAt = DateTime.UtcNow
            });
            db.SaveChanges();

            // Вторая вставка с тем же email упадёт из-за уникального индекса
            db.Users.Add(new User
            {
                Name = "Второй",
                Email = "duplicate@test.com",
                CreatedAt = DateTime.UtcNow
            });
            db.SaveChanges();

            transaction.Commit();
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            Console.WriteLine($"  Поймали исключение: {ex.GetType().Name}");
            Console.WriteLine("  Откат выполнен — в БД не появилось ни одной записи этой транзакции.");
        }
    }
}