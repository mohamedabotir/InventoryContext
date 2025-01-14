using Common.Domains;
using Common.Entity;
using Common.Handlers;
using Common.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Context;

public class UnitOfWork(ItemContext dbContext, IServiceProvider serviceProvider) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync(IEnumerable<DomainEventBase> events,CancellationToken cancellationToken = default)
    {
     
        var result = await dbContext.SaveChangesAsync(cancellationToken);
        return result;
    }


    public IRepository<T>? GetRepository<T>() where T : AggregateRoot
    {
        return serviceProvider.GetService<IRepository<T>>();
    }
    public void Dispose()
    {
        dbContext?.Dispose();
        GC.SuppressFinalize(this);
    }
}