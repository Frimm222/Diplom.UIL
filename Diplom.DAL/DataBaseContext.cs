using Diplom.BLL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Windows;

public class DataBaseContext : DbContext
{
    private readonly string _connectionString;
    public DbSet<Item> table_items { get; set;}
    public DbSet<Category> table_category { get; set; }
    public DbSet<User> table_users {get; set; }
    public DbSet<Log> table_logging { get; set; }
    public DataBaseContext()
    {
        _connectionString = File.ReadAllText(Path.GetFullPath("app.db"));
        Database.EnsureCreated();
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_connectionString);
        optionsBuilder.EnableSensitiveDataLogging();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(p => p.id);
        });
        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(p => p.item_id);
        });

        base.OnModelCreating(modelBuilder);
    }
}