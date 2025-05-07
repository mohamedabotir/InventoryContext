using Common.Events;
using Domain.Entities;

namespace Application.Handlers;

public class EventHandler(Domain.Repository.IEventRepository eventRepository):IEventHandler
{
  

    public async Task On(OrderShipped @event)
    {
        //validate if status 
        //produce order closed
        var eventModel = new EventModel
        {
            AggregateIdentifier = @event.PoNumber,
            Version = 0,
            EventBaseData = @event,
            EventType = nameof(OrderShipped),
            AggregateType = nameof(Item),
            TimeStamp = DateTime.Now
        };
       await eventRepository.SaveEventAsync(eventModel);// it has to be produces to those topics["purchaseOrder", "shippingOrder"]
    }
}