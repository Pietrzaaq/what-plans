using AspNetCore.Identity.Mongo.Model;

namespace WhatPlans.Domain.Entities;

public class User : MongoUser
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Culture { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsOrganizer { get; set; }
    public DateTime RegisterDate { get; set; } 
    public DateTime LastVisitDate { get; set; } 
    public string AvatarUrl { get; set; }
}

public class UserDto
{
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateOnly BirthDate { get; set; }
    public string Culture { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsOrganizer { get; set; }
    public DateTime RegisterDate { get; set; } 
    public DateTime LastVisitDate { get; set; } 
    public string AvatarUrl { get; set; }
}

public class UserLightDto
{
    public string Username { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public bool IsAdmin { get; set; }
    public bool IsOrganizer { get; set; }
    public string AvatarUrl { get; set; }
}