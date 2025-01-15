using Domain.Repository;
using GraphQL;
using GraphQL.Types;
using Inventory.API.GraphQL.Types;

namespace Inventory.API.GraphQL.Query;

public class ItemQuery:ObjectGraphType
{
    public ItemQuery(IItemRepository itemRepository)
    {
        Field<ListGraphType<ItemType>>("items")
            .ResolveAsync(async context =>await itemRepository.GetAllAsync());
        Field<ItemType>("item")
            .Arguments(new QueryArguments(new QueryArgument<NonNullGraphType<IdGraphType>> { Name = "id" }))
            .ResolveAsync(async context =>
            {
                var itemId = context.GetArgument<int>("id");
                return await itemRepository.GetByIdAsync(itemId);
            });
        
    }
}