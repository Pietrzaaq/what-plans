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
    
    public static bool operator ==(EntityId a, string b)
        => a?.Value == b;    
    
    public static bool operator !=(EntityId a, string b)
        => a?.Value != b;
    
    public static bool operator ==(EntityId a, EntityId b)
        => a?.Value == b?.Value;    
    
    public static bool operator !=(EntityId a, EntityId b)
        => a?.Value != b?.Value;
    
    public override string ToString() 
        => Value;
    
    protected bool Equals(EntityId other)
    {
        return Value == other.Value;
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((EntityId)obj);
    }

    public override int GetHashCode()
    {
        return (Value != null ? Value.GetHashCode() : 0);
    }
}