using InventoryService.Entities;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Data;

public class InventoryDbContext : DbContext
{
    public InventoryDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<InventoryItem> InventoryItems { get; set; }
}
