using System.Text.Json;
using System.Text.Json.Serialization;
using Common.Events;
using Common.Serializer;

namespace Infrastructure.Consumer;

public class EventJsonConverter: JsonConverter<DomainEventBase>
{
    public override bool CanConvert(Type typeToConvert)
    {
        return typeToConvert.IsAssignableFrom(typeof(DomainEventBase));
    }
    public override DomainEventBase? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (!JsonDocument.TryParseValue(ref reader, out var doc))
            throw new JsonException($"Can not Convert Type {typeof(JsonDocument)}");

        if (!doc.RootElement.TryGetProperty("Type", out var type))
            throw new JsonException($"Type not Complete {typeof(JsonDocument)}");
        var disc = type.GetString();
        var json = doc.RootElement.GetRawText();
        return disc switch
        {
            nameof(OrderShipped) => JsonSerializer.Deserialize<OrderShipped>(json, options),
            _ => throw new JsonException($"Event Type {disc} not supported yet!")
        };

    }


    public override void Write(Utf8JsonWriter writer, DomainEventBase value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
