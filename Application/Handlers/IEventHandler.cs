using Common.Events;

namespace Application.Handlers;

public interface IEventHandler
{
    public Task On(OrderShipped @event);

}