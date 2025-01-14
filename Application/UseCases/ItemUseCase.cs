using Application.Dtos;
using Application.Models;
using Common.Repository;
using Common.Result;
using Common.ValueObject;
using Domain.Entities;
using Domain.Repository;
using Domain.ValueObject;

namespace Application.UseCases;

public class ItemUseCase(IItemRepository itemRepository,IUnitOfWork unitOfWork): IItemUseCase
{
    public async Task<Result> Create(CreateItemCommand item)
    {
        using (unitOfWork)
        {
            var price = Money.CreateInstance(item.Price);
            var name  = Domain.ValueObject.Name.CreateInstance(item.Name);
            var description = Domain.ValueObject.Description.CreateInstance(item.Description);
            var sku = SKU.CreateInstance();
            var stocks = item.Stocks.Select(MapItemStockDtoToStock).ToList();
            var result = Result.Combine(price, name, description);
            if (result.IsFailure)
            {
                return Result.Fail(result.Message);
            }
            var createdItem = new Item(name.Value, description.Value, price.Value, sku,stocks);
           await itemRepository.AddAsync(createdItem);
           await unitOfWork.SaveChangesAsync(createdItem.DomainEvents);
            return Result.Ok();
        }
    }

    private Stock MapItemStockDtoToStock(ItemStockDto value)
    {
        
        return new Stock(value.Id,0,value.Quantity,value.Location);
    }
}