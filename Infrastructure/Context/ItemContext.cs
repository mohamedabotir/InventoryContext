using Infrastructure.Context.Pocos;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class ItemContext: DbContext
{
    public ItemContext(DbContextOptions<ItemContext> options) : base(options)
    {
        
    }
    public DbSet<ItemPoco> Items { get; set; }
}