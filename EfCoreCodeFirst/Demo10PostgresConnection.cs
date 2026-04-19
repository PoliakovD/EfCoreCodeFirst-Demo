using EfCoreCodeFirst.DAL;
using Microsoft.EntityFrameworkCore;
using System;

namespace EfCoreCodeFirst;

/// <summary>
/// Класс для демонстрации подключения к PostgreSQL с использованием Entity Framework Core
/// </summary>
public class Demo10PostgresConnection
{
    /// <summary>
    /// Метод демонстрирует различные способы подключения к PostgreSQL
    /// </summary>
    public static void Run()
    {
        Console.WriteLine("=== Демонстрация подключения к PostgreSQL ===\n");
        
        // Способ 1: Создание контекста через фабрику (рекомендуемый способ)
        var db = new DbContextFactory().CreateDbContext(new string[0]);
        
        try
        {
            // Проверка возможности подключения
            bool canConnect = db.Database.CanConnect();
            Console.WriteLine($"Подключение к PostgreSQL возможно: {canConnect}");
            
            if (canConnect)
            {
                // Получение информации о базе данных
                var dbInfo = db.Database.ProviderName;
                Console.WriteLine($"Провайдер базы данных: {dbInfo}");
                
                // Применение миграций
                db.Database.Migrate();
                Console.WriteLine("Миграции применены успешно");
                
                // Проверка наличия данных
                var userCount = db.Users.Count();
                Console.WriteLine($"Количество пользователей в базе: {userCount}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при подключении к PostgreSQL: {ex.Message}");
            
            // Более детальная информация об ошибке
            if (ex.InnerException != null)
            {
                Console.WriteLine($"Внутренняя ошибка: {ex.InnerException.Message}");
            }
        }
        finally
        {
            // Важно освободить ресурсы
            db.Dispose();
        }
        
        Console.WriteLine("\n=== Демонстрация завершена ===");
    }
}"