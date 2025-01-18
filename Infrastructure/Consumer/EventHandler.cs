using Common.Events;
using Domain.Repository;

namespace Infrastructure.Consumer;

public class EventHandler(IItemRepository purchaseOrderRepository,IEventStore eventStore):IEventHandler
{
  

    public async Task On(OrderShipped @event)
    {
        //validate if status 
        //produce order closed
       await eventStore.SaveEventAsync(@event.PurchaseOrderGuid, new OrderClosed(@event.PurchaseOrderGuid, @event.PoNumber),
            ["purchaseOrder", "shippingOrder"]);
    }
}