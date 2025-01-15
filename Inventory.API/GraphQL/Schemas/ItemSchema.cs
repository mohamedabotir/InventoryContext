using GraphQL.Types;
using Inventory.API.GraphQL.Query;

namespace Inventory.API.GraphQL.Schemas;

public class ItemSchema:Schema
{
    public ItemSchema(IServiceProvider serviceProvider):base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<ItemQuery>();

    }
}