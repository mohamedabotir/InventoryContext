using Common.Repository;
using Domain.Entities;

namespace Domain.Repository;

public interface IItemRepository : IRepository<Item>
{
    Task<ILookup<int,Stock>> GetAllStocks(IEnumerable<int> itemId);
}
