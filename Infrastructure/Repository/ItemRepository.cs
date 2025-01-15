using System.Linq.Expressions;
using Domain.Entities;
using Domain.Repository;
using Infrastructure.Context;
using Infrastructure.Context.Pocos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class ItemRepository:IItemRepository
{
    private readonly ItemContext _dbContext;
    public ItemRepository(ItemContext context)
    {
        _dbContext = context;
        _dbContext.Set<Item>();
    }
    public async Task<Item> GetByIdAsync(int id)
    {
        var result =await _dbContext.Items.Where(e=>e.Id == id)
            .Include(e=>e.StockItems)
            .Select(e => e.MapItemPocoToItem(e))
            .FirstOrDefaultAsync();
        result = result is { IsFailure: false } ? result : null;

        return result!.Value;
    }

    public Task<IEnumerable<Item>> GetAllAsync()
    {
        var result = _dbContext.Items.Include(e=>e.StockItems)
            .Select(e => e.MapItemPocoToItem(e)).ToList();
        result = result.Where(e => e.IsFailure==false).ToList();
       return Task.FromResult<IEnumerable<Item>>(result.Select(e=>e.Value));
    }

    public async Task AddAsync(Item entity)
    {
        var item = new ItemPoco();
       await _dbContext.Set<ItemPoco>().AddAsync(item.MapItemToItemPoco(entity));
    }

    public Task UpdateAsync(Item entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Item>> FindAsync(Expression<Func<Item, bool>> predicate)
    {
        throw new NotImplementedException();
    }

    public async Task<ILookup<int,Stock>> GetAllStocks(IEnumerable<int> itemId)
    {
        var result =await _dbContext.Items.Where(e=>itemId.Contains(e.Id))
            .Include(e=>e.StockItems)
            .Select(e => e.MapItemPocoToItem(e))
             .ToListAsync();
        result = result.Where(e => e.IsFailure==false).ToList();

        var stockLookup = result.Select(e => e.Value)
            .ToList().SelectMany(e=>e.Stocks)
            .ToList().ToLookup(e=>e.ItemId);
        return stockLookup;
    }
}