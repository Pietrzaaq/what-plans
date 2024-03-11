namespace WhatPlans.Domain.Entities;

public class Organizer
{
    public Guid Id { get; set; }
    public Guid? PersonId { get; set; }
    public Guid? CompanyId { get; set; }
    public string Name { get; set; }
}