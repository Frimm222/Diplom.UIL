using Diplom.BLL.Models;
using Microsoft.EntityFrameworkCore;
using System.IO;

public class DataBaseContext : DbContext
{
    private readonly string _connectionString;
    public DbSet<Item> table_items { get; set;}
    public DbSet<Category> table_category { get; set; }
    public DbSet<User> Users => Set<User>();
    public DataBaseContext()
    {
        _connectionString = File.ReadAllText(Path.GetFullPath("app.db"));
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
    }
}