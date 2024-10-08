﻿using MongoDB.Bson;

namespace WhatPlans.Domain.Entities;

public class User
{
    public ObjectId Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
}