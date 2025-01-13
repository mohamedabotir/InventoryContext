using Common.Entity;
using Common.ValueObject;
using Domain.ValueObject;

namespace Domain.Entities;

public class Item  : AggregateRoot
{
    public Item(Name name, Description description, Money price, SKU sku, ICollection<Stock> stocks)
    {
        Name = name;
        Description = description;
        Price = price;
        SKU = sku;
        Stocks = stocks;
    }

    public Name Name { get;protected set; }
    public Description Description { get;protected set; } 
    public Money Price { get;protected set; }  
    public SKU SKU { get;protected set; } 
    public ICollection<Stock> Stocks { get;protected set; }

    public Item()
    {
        
    }
}  
