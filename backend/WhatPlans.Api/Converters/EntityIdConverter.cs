using Newtonsoft.Json;
using WhatPlans.Domain.Common;

namespace WhatPlans.Api.Converters;

public class EntityIdConverter : JsonConverter<EntityId>
{
    public override void WriteJson(JsonWriter writer, EntityId value, JsonSerializer serializer)
    {
        writer.WriteValue(value.Value);
    }

    public override EntityId ReadJson(JsonReader reader, Type objectType, EntityId existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var stringValue = reader.Value?.ToString();
        return new EntityId(stringValue);
    }
}