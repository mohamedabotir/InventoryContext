using Common.Events;
using Common.Repository;
using Common.Result;
using Domain.Entities;
using Infrastructure.MessageBroker;
using Infrastructure.MessageBroker.Producers;
using Microsoft.Extensions.Options;

namespace Infrastructure.Mongo;

public class InventoryEventStore(IEventRepository eventRepository, IProducer producer, IOptions<InventoryTopic> options) : IEventStore
{

    public async Task SaveEventAsync(Guid aggregateId, DomainEventBase eventBase,List<Maybe<string>> anotherTopics)
    {
 
            var eventModel = new EventModel
            {
                Id = Guid.NewGuid(),
                TimeStamp = DateTime.Now,
                AggregateType = nameof(Item),
                AggregateIdentifier = aggregateId,
                EventBaseData = eventBase,
                EventType = eventBase.GetType().Name
            };
            await eventRepository.SaveEventAsync(eventModel);

            var topicName = options.Value.TopicName;
            await producer.ProduceAsync(topicName, eventBase);
            var shipmentTopics = anotherTopics.Where(e=>e.HasValue)
                .Select(e => e.Value).ToList();
            foreach (var topic in shipmentTopics)
            {
                await producer.ProduceAsync(topic, eventBase);
            }
    }

    public Task<List<DomainEventBase>> GetEventsAsync(Guid aggregateId, string collectionName = "")
    {
        throw new NotImplementedException();
    }

}