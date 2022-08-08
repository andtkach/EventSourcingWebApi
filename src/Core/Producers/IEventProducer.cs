using Core.Events;

namespace Core.Producers
{
    public interface IEventProducer
    {
        Task ProduceAsync<T>(string topic, T evt) where T : BaseEvent;
    }
}