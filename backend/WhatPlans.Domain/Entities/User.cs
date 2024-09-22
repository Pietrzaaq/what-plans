using MongoDB.Bson;
using WhatPlans.Domain.Common;

namespace WhatPlans.Domain.Entities;

public class User
{
    public EntityId Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
}