using EfCoreCodeFirst.DAL;
using EfCoreCodeFirst.DAL.Models;

namespace EfCoreCodeFirst;

public class Demo3Crud
{
    public static void Run()
    {
        Console.WriteLine("=== Блок 3. CRUD ===");
        
        using var db = new DbContextFactory().CreateDbContext(["log"]);
        
        // CREATE — вставка одного пользователя
        var newUser = new User()
        {
            Name = "Иван",
            Email = "ivan@example.com",
           
        };
        db.Users.Add(newUser);
        db.SaveChanges();

        Console.WriteLine($"Пользователь {newUser.Name} добавлен с id: {newUser.Id}");
        
        // READ
        
        var user = db.Users.FirstOrDefault(u => u.Id == newUser.Id);
        Console.WriteLine($"Считан Пользователь {user.Name} с id: {user.Id}");
        
        //UPDATE
        user.Name = "Олег";
        db.Users.Update(user);
        db.SaveChanges();
        
        user = db.Users.FirstOrDefault(u => u.Id == newUser.Id);
        Console.WriteLine($"Обновлен Пользователь {user.Name} с id: {user.Id}");
        
        // DELETE — удаление
        db.Users.Remove(user);
        db.SaveChanges();
        Console.WriteLine(" Пользователь  удален");
        
        

    }
}