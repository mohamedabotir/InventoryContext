using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

namespace Infrastructure.Context.Pocos;
[Table("Items")]
public class ItemPoco
{



    public ItemPoco MapItemToItemPoco(Item item)
    {
        return this;
    }
}