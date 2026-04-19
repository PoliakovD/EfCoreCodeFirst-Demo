using EfCoreCodeFirst.DAL;
using Microsoft.EntityFrameworkCore;

namespace EfCoreCodeFirst;

public class Demo1Connection
{
    public static void Run()
    {
        var db = new DbContextFactory().CreateDbContext(["log"]);
        
        
        bool canConnect = db.Database.CanConnect();
        Console.WriteLine($"База данных доступна: {canConnect}");
        
        // bool created = db.Database.EnsureCreated();
        // Console.WriteLine($"База только что создана этим вызовом: {created}");
        
        db.Database.Migrate();
        
        
        var users = db.Users.ToList();
        Console.WriteLine($"Пользователей {users.Count} users");
        
    }
}