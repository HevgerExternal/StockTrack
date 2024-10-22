namespace InventoryService.DTOs.InventoryItem;

public class UpdateInventoryItemDto
{
    public string Name { get; set; }
    public int? Quantity { get; set; }
    public string Location { get; set; }
}
