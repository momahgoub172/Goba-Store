using Goba_Store.Models;
using Microsoft.EntityFrameworkCore;

namespace Goba_Store.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    
    public DbSet<Category> Categories { get; set; }
    public DbSet<Product>  Products { get; set; }
}