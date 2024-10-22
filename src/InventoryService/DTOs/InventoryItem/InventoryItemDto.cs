namespace InventoryService.DTOs;

public class InventoryItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string ItemNumber { get; set; }
    public int Quantity { get; set; }
    public string Location { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}
