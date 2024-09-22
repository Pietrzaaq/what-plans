using MongoDB.Bson;

namespace WhatPlans.Domain.Entities;

public class Organizer
{
    public ObjectId Id { get; set; }
    public ObjectId? UserId { get; set; }
    public ObjectId? CompanyId { get; set; }
    public string Name { get; set; }
}