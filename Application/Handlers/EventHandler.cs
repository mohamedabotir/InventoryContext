using Common.Events;
using Domain.Entities;
using Domain.Events;

namespace Application.Handlers;

public class EventHandler(Domain.Repository.IEventRepository eventRepository):IEventHandler
{
  

    public async Task On(OrderShipped @event)
    {
        //validate if status 
        //produce order closed
        var closedEvent = new OrderClosed(@event.PurchaseOrderGuid, @event.PoNumber);
        closedEvent.Version = 0;
        var eventModel = new EventModel
        {
            AggregateIdentifier = @event.PoNumber,
            Version = 0,
            EventBaseData = closedEvent,
            EventType = nameof(OrderClosed),
            AggregateType = nameof(Item),
            TimeStamp = DateTime.Now
        };
       await eventRepository.SaveEventAsync(eventModel);// it has to be produces to those topics["purchaseOrder", "shippingOrder"]
    }
}