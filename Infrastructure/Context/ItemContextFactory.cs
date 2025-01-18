using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Context;

public class ItemContextFactory
{
    private readonly Action<DbContextOptionsBuilder> _configureDbContext;

    public ItemContextFactory(Action<DbContextOptionsBuilder> configureDbContext)
    {
        _configureDbContext = configureDbContext;
    }

    public ItemContext CreateDataBaseContext()
    {

        DbContextOptionsBuilder<ItemContext> optionsBuilder = new();
        _configureDbContext(optionsBuilder);


        return new ItemContext(optionsBuilder.Options);

    }
}