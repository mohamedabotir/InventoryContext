using Common.Events;

namespace Domain.Events;

public interface IEventHandler
{
    public Task On(OrderShipped @event);

}