using System.Text.Json;
using Confluent.Kafka;
using Core.Consumers;
using Core.Events;
using Microsoft.Extensions.Options;
using Statement.Query.Infrastructure.Converters;
using Statement.Query.Infrastructure.Handlers;

namespace Statement.Query.Infrastructure.Consumers
{
    public class EventConsumer : IEventConsumer
    {
        private readonly ConsumerConfig _config;
        private readonly IEventHandler _eventHandler;

        public EventConsumer(
            IOptions<ConsumerConfig> config,
            IEventHandler eventHandler)
        {
            _config = config.Value;
            _eventHandler = eventHandler;
        }

        public void Consume(string topic)
        {
            using var consumer = new ConsumerBuilder<string, string>(_config)
                    .SetKeyDeserializer(Deserializers.Utf8)
                    .SetValueDeserializer(Deserializers.Utf8)
                    .Build();

            consumer.Subscribe(topic);

            while (true)
            {
                var consumeResult = consumer.Consume();

                if (consumeResult?.Message == null) continue;

                var options = new JsonSerializerOptions { Converters = { new EventJsonConverter() } };
                var evt = JsonSerializer.Deserialize<BaseEvent>(consumeResult.Message.Value, options);
                var handlerMethod = _eventHandler.GetType().GetMethod("On", new Type[] { evt.GetType() });

                if (handlerMethod == null)
                {
                    throw new ArgumentNullException(nameof(handlerMethod), "Could not find event handler method");
                }

                handlerMethod.Invoke(_eventHandler, new object[] { evt });
                consumer.Commit(consumeResult);
            }
        }
    }
}
