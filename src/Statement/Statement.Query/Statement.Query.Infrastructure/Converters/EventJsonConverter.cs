using System.Text.Json;
using System.Text.Json.Serialization;
using Core.Events;
using Statement.Common.Events;

namespace Statement.Query.Infrastructure.Converters
{
    public class EventJsonConverter : JsonConverter<BaseEvent>
    {
        public override bool CanConvert(Type type)
        {
            return type.IsAssignableFrom(typeof(BaseEvent));
        }

        public override BaseEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (!JsonDocument.TryParseValue(ref reader, out var doc))
            {
                throw new JsonException($"Failed to parse {nameof(JsonDocument)}");
            }

            if (!doc.RootElement.TryGetProperty("Type", out var type))
            {
                throw new JsonException("Could not detect the Type discriminator property");
            }

            var typeDiscriminator = type.GetString();
            var json = doc.RootElement.GetRawText();

            return typeDiscriminator switch
            {
                nameof(StatementCreatedEvent) => JsonSerializer.Deserialize<StatementCreatedEvent>(json, options),
                nameof(StatementUpdatedEvent) => JsonSerializer.Deserialize<StatementUpdatedEvent>(json, options),
                nameof(StatementLikedEvent) => JsonSerializer.Deserialize<StatementLikedEvent>(json, options),
                nameof(CommentAddedEvent) => JsonSerializer.Deserialize<CommentAddedEvent>(json, options),
                nameof(CommentUpdatedEvent) => JsonSerializer.Deserialize<CommentUpdatedEvent>(json, options),
                nameof(CommentRemovedEvent) => JsonSerializer.Deserialize<CommentRemovedEvent>(json, options),
                nameof(StatementRemovedEvent) => JsonSerializer.Deserialize<StatementRemovedEvent>(json, options),
                _ => throw new JsonException($"{typeDiscriminator} is not supported yet")
            };
        }

        public override void Write(Utf8JsonWriter writer, BaseEvent value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}