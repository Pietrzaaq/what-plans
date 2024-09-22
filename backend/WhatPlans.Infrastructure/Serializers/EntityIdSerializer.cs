using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using WhatPlans.Domain.Common;

namespace WhatPlans.Infrastructure.Serializers;

public class EntityIdSerializer : SerializerBase<EntityId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, EntityId entityId)
    {
        context.Writer.WriteString(entityId.Value);
    }

    public override EntityId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var objectId = context.Reader.ReadObjectId();
        return new EntityId(objectId.ToString());
    }
}