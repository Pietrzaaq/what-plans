using MongoDB.Bson;
using Newtonsoft.Json;


namespace WhatPlans.Api.Converters;

public class ObjectIdConverter : JsonConverter<ObjectId>
{
    public override void WriteJson(JsonWriter writer, ObjectId value, JsonSerializer serializer)
    {
        writer.WriteValue(value.ToString());
    }

    public override ObjectId ReadJson(JsonReader reader, Type objectType, ObjectId existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        var stringValue = reader.Value?.ToString();
        return new ObjectId(stringValue);
    }
}