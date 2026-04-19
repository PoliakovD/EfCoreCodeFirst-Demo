namespace EfCoreCodeFirst.DAL.Models;

public record Product
{
    public int Id { get; set; }
    public string Title { get; set; }
    public decimal Price { get; set; }
    
    // связь многие к одному
    public int UserId { get; set; }
    public User User { get; set; }
    
    // связь многие к одному
    public int? CategoryId { get; set; }
    public Category Category { get; set; }
}