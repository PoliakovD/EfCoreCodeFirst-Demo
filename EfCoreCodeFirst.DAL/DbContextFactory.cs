using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;


    namespace EfCoreCodeFirst.DAL;

    public class DbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
    public AppDbContext CreateDbContext(string[] args=null)
    {
        var config = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        
        var connection = config.GetConnectionString("DefaultConnection");
        
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();
        
        optionsBuilder.UseNpgsql(connection);
        
        if(args!=null && args.Length>0 && args[0]=="log")
            optionsBuilder.LogTo(Console.WriteLine,LogLevel.Information)
                .EnableSensitiveDataLogging();
        
        return new AppDbContext(optionsBuilder.Options);
    }
}