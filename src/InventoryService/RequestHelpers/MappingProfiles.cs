using AutoMapper;
using InventoryService.DTOs;
using InventoryService.DTOs.InventoryItem;
using InventoryService.Entities;

namespace InventoryService.RequestHelpers;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<InventoryItem, InventoryItemDto>();
        CreateMap<CreateInventoryItemDto, InventoryItem>();
    }
}
