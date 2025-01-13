using System.Linq.Expressions;
using Domain.Entities;
using Domain.Repository;
using Infrastructure.Context.Pocos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repository;

public class ItemRepository(DbContext context):IItemRepository
{
    public Task<Item> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Item>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public async Task AddAsync(Item entity)
    {
        var item = new ItemPoco();
       await context.Set<ItemPoco>().AddAsync(item.MapItemToItemPoco(entity));
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
}