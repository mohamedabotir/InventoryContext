using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Common.ValueObject;
using Domain.Entities;

namespace Infrastructure.Context.Pocos;
[Table("Stock")]
public class StockPoco
{ 
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public Guid Guid { get; set; }
    public QuantityType QuantityType { get; set; }
    public int Quantity { get; set; }
    [StringLength(50)]
    public string Location { get; set; }
    public int ItemId { get; set; }
    [ForeignKey("ItemId")]
    public ItemPoco Item { get; set; }

    public StockPoco MapStockToStockPoco(Stock stock)
    {
        Id = (int)stock.Id;
        Guid = stock.Guid;
        QuantityType = stock.Quantity.QuantityType;
        Quantity = stock.Quantity.QuantityValue;
        Location = stock.Location;
        ItemId = stock.ItemId;
        return this;
    }
    public Stock MapStockPocoToStock(StockPoco stock)=>new Stock(stock.Id,stock.Guid,stock.ItemId, Common.ValueObject.Quantity.CreateInstance(stock.Quantity,QuantityType).Value, stock.Location);
    
}