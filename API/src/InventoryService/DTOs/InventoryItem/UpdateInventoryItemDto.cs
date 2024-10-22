using System.ComponentModel.DataAnnotations;

namespace InventoryService.DTOs.InventoryItem;

public class UpdateInventoryItemDto
{
    [Required]
    public string Name { get; set; }
    
    [Required]
    public int? Quantity { get; set; }
    
    [Required]
    public string Location { get; set; }
}
