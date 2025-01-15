using Domain.Entities;
using Domain.Repository;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;

namespace Inventory.API.GraphQL.Types;

public class ItemType:ObjectGraphType<Item>
{
    public ItemType(IDataLoaderContextAccessor accessor, IItemRepository dbContext)
    {
        Field(x => x.Id).Description("The ID of Item.");
        Field(x => x.Description.DescriptionValue).Description("Item Description.");
        Field(x => x.Name.NameValue).Description("Item Name.");
        Field(x => x.SKU.SKUValue).Description("Stock keeping unit.");
        Field(x => x.Price.MoneyValue).Description("Item Price.");
        Field(x => x.CreatedOn).Description("Item Created Date.");
        Field(x => x.ModifiedOn).Description("Last Modified Date.");
        Field(x => x.Guid).Description("Order Guid");
       Field<ListGraphType<StockType>>("stocks").Description("The list of line items.");
      /* Field<ListGraphType<StockType>>("stocks")
           .Resolve(context =>
           {
               var loader = accessor.Context.GetOrAddCollectionBatchLoader<int, Stock>(
                   "GetStocksByItemId",dbContext.GetAllStocks);

               return loader.LoadAsync((int)context.Source.Id);
           });
           */
    }
}