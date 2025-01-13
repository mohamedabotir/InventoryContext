using Common.Entity;
using Common.ValueObject;

namespace Domain.Entities;

public class Stock  :Entity
{
    public Stock(Guid guid, int itemId, Quantity quantity, string location)
    {
        Guid = guid;
        ItemId = itemId;
        Quantity = quantity;
        Location = location;
    }

    public Guid Guid { get;protected set; }
    public int ItemId { get;protected set; } 
    public Quantity Quantity { get;protected set; } 
    public string Location { get;protected set; }
}  
