using Domain.Entities;
using GraphQL.Types;

namespace Inventory.API.GraphQL.Types;

public class StockType:ObjectGraphType<Stock>
{
    public StockType()
    {
        Field(x => x.Id).Description("The ID of Item.");    
        Field(x => x.Guid).Description("Guid of Item.");
        Field(x => x.Location).Description("Location Address.");
        Field(x => x.Quantity.QuantityValue).Description("Quantity Value.");
        Field(x => x.Quantity.QuantityType).Description("Quantity Type.");
        Field(x => x.CreatedOn).Description("Item Created Date.");
        Field(x => x.ModifiedOn).Description("Item Modified Date.");

    }
}