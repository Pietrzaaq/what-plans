using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using WhatPlans.Domain.Common;

namespace WhatPlans.Infrastructure.Serializers;

public class EntityIdSerializer : SerializerBase<EntityId>
{
    public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, EntityId entityId)
    {
        if (entityId is null || string.IsNullOrEmpty(entityId.Value))
        {
            context.Writer.WriteNull();
        }
        else
        {
            context.Writer.WriteString(entityId.Value);
        }
    }

    public override EntityId Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
    {
        var bsonType = context.Reader.CurrentBsonType;
        switch (bsonType)
        {
            case BsonType.ObjectId:
                var objectId = context.Reader.ReadObjectId();
                return new EntityId(objectId.ToString());
            case BsonType.String:
                var stringId = context.Reader.ReadString();
                return new EntityId(stringId);
            case BsonType.Null:
                context.Reader.ReadNull();
                return null;
            default:
                throw new NotSupportedException($"BsonType {bsonType} is not supported");
        }
    }
}