using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace WhatPlans.Domain.Common;

public class EntityId
{
    [BsonRepresentation(BsonType.ObjectId)]
    public string Value { get; }

    public EntityId()
    {
        Value = ObjectId.GenerateNewId().ToString();
    }

    public EntityId(string id)
    {
        Value = id;
    }
    
    public static implicit operator string(EntityId entityId)
        => entityId.Value;
        
    public static implicit operator EntityId(string id)
    {
        return new EntityId(id);
    }
}