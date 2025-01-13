using Common.ValueObject;

namespace Application.Dtos;

public record ItemStockDto(Guid Id,string Location,Quantity Quantity);