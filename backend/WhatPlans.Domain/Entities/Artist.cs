﻿namespace WhatPlans.Domain.Entities;

public class Artist
{
    public Guid Id { get; set; }
    public Guid PersonId { get; set; }
    public string Name { get; set; }
}