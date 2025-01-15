using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.Result;
using Common.ValueObject;
using Domain.Entities;

namespace Infrastructure.Context.Pocos;
[Table("Item")]
public class ItemPoco
{   
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    [Required]
    public Guid Guid { get; set; }
    [Required]
    [StringLength(126, MinimumLength = 10, ErrorMessage = "Name must be between 10 and 126 characters.")]
    public string Name { get; set; }
    [Required]
    [StringLength(500, MinimumLength = 50, ErrorMessage = "Name must be between 50 and 500 characters.")]
    public string Description { get; set; } 
    [Required] 
    [Column(TypeName = "decimal(18, 2)")] 
    public decimal Price { get; set; }  
    public string SKU { get; set; }
    public DateTime CreatedOn { get; set; }
    public DateTime? LastModification { get; set; }
    public  virtual ICollection<StockPoco> StockItems { get; protected set; }


    public ItemPoco MapItemToItemPoco(Item item)
    {
        Id = (int)item.Id;
        Guid = item.Guid;
        Name = item.Name.NameValue;
        Description = item.Description.DescriptionValue;
        Price = item.Price.MoneyValue;
        SKU = item.SKU.SKUValue;
        CreatedOn = DateTime.UtcNow;
        LastModification = item.ModifiedOn;
        StockItems = new List<StockPoco>();
        var stocks = item.Stocks.Select(new StockPoco().MapStockToStockPoco).ToList();
        foreach (var stock in stocks)
        {
            StockItems.Add(stock);
        }
        return this;
    }
    public Result<Item> MapItemPocoToItem(ItemPoco item)
    {
        var price = Money.CreateInstance(item.Price);
        var name  = Domain.ValueObject.Name.CreateInstance(item.Name);
        var description = Domain.ValueObject.Description.CreateInstance(item.Description);
        var sku = Domain.ValueObject.SKU.IsValidSku(item.SKU);
        var stocks = item.StockItems.Select(e=>e.MapStockPocoToStock(e)).ToList();
        var result = Result.Combine(price, name, description,sku);
        if (result.IsFailure)
        {
            return  Result.Fail<Item>(result.Message);
        }
        var createdItem = new Item(item.Guid,item.CreatedOn,item.LastModification,item.Id,name.Value, description.Value, price.Value, sku.Value,stocks);
      
        return  Result.Ok(createdItem);;
    }
}