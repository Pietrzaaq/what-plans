using MongoDB.Bson;

namespace WhatPlans.Domain.Entities;

public class Artist
{
    public ObjectId Id { get; set; }
    public ObjectId PersonId { get; set; }
    public string Name { get; set; }
}