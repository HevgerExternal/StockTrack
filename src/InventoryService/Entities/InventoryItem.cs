using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryService.Entities;

[Table("InventoryItems")]
public class InventoryItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ItemNumber { get; set; }
    public int Quantity { get; set; }
    public string Location { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
