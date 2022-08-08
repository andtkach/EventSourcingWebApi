using Core.Events;

namespace Core.Domain
{
    public interface IEventStoreRepository
    {
        Task SaveAsync(EventModel evt);
        Task<List<EventModel>> FindByAggregateId(Guid aggregateId);
        Task<List<EventModel>> FindAllAsync();
    }
}