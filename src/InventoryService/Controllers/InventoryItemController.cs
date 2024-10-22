using AutoMapper;
using InventoryService.Data;
using InventoryService.DTOs;
using InventoryService.DTOs.InventoryItem;
using InventoryService.Entities;
using InventoryService.RequestHelpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryService.Controllers;

[ApiController]
[Route("api/inventory-items")]
public class InventoryItemController : ControllerBase
{
    private readonly InventoryDbContext _context;
    private readonly IMapper _mapper;

    public InventoryItemController(InventoryDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<List<InventoryItemDto>>> GetAllInventoryItems([FromQuery] SearchParams searchParams)
    {
        IQueryable<InventoryItem> query = _context.InventoryItems;

        if (!string.IsNullOrWhiteSpace(searchParams.SearchTerm))
        {
            var normalizedSearchTerm = searchParams.SearchTerm.ToLower();
            query = query.Where(item => EF.Functions.ILike(item.Name, $"%{normalizedSearchTerm}%"));
        }

        query = searchParams.OrderBy?.ToLower() switch
        {
            "location" => query.OrderBy(item => item.Location),
            "createdat" => query.OrderBy(item => item.CreatedAt),
            "updatedat" => query.OrderBy(item => item.UpdatedAt),
            _ => query
        };

        int totalCount = await query.CountAsync();
        var items = await query.Skip((searchParams.PageNumber - 1) * searchParams.PageSize)
                            .Take(searchParams.PageSize)
                            .ToListAsync();

        var itemDtos = _mapper.Map<List<InventoryItemDto>>(items);
        int pageCount = (int)Math.Ceiling(totalCount / (double)searchParams.PageSize);

        return Ok(new
        {
            result = itemDtos,
            currentPage = searchParams.PageNumber,
            pageCount = pageCount,
            totalCount = totalCount
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<InventoryItemDto>> GetInventoryItemById(Guid id)
    {
        var inventoryItem = await _context.InventoryItems
            .FirstOrDefaultAsync(x => x.Id == id);
        
        if(inventoryItem == null) return NotFound();

        return _mapper.Map<InventoryItemDto>(inventoryItem);
    }

    [HttpPost]
    public async Task<ActionResult<InventoryItemDto>> CreateInventoryItem(CreateInventoryItemDto inventoryItemDto)
    {
        var existingItem = await _context.InventoryItems
            .AnyAsync(item => item.ItemNumber == inventoryItemDto.ItemNumber);

        if (existingItem)
        {
            return BadRequest("An item with the same item number already exists.");
        }
        
        var inventoryItem = _mapper.Map<InventoryItem>(inventoryItemDto);
        _context.InventoryItems.Add(inventoryItem);

        var result = await _context.SaveChangesAsync() > 0;

        if(!result) return BadRequest("Could not save changes");

        return CreatedAtAction(nameof(GetInventoryItemById),
            new { inventoryItem.Id }, _mapper.Map<InventoryItemDto>(inventoryItem));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateInventoryItem(Guid id, UpdateInventoryItemDto updateInventoryItemDto)
    {
        var inventoryItem = await _context.InventoryItems
            .FirstOrDefaultAsync(x => x.Id == id); 
        
        if(inventoryItem == null) return NotFound();

        inventoryItem.Name = updateInventoryItemDto.Name ?? inventoryItem.Name;
        inventoryItem.Quantity = updateInventoryItemDto.Quantity ?? inventoryItem.Quantity;
        inventoryItem.Location = updateInventoryItemDto.Location ?? inventoryItem.Location;
        inventoryItem.UpdatedAt = DateTime.UtcNow;

        var result = await _context.SaveChangesAsync() > 0;

        if(result) return Ok();

        return BadRequest("Could not save changes");
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteInventoryItem(Guid id)
    {
        var inventoryItem = await _context.InventoryItems
            .FindAsync(id); 
        
        if(inventoryItem == null) return NotFound();

        _context.InventoryItems.Remove(inventoryItem);

        var result = await _context.SaveChangesAsync() > 0;

        if(!result) return BadRequest("Could not delete item");

        return Ok();
    }
}
