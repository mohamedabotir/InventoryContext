using Common.Events;

namespace Infrastructure.Consumer;

public interface IEventHandler
{
    public Task On(OrderShipped @event);

}