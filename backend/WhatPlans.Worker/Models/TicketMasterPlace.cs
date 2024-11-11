namespace WhatPlans.Worker.Models;

public class TicketMasterPlace
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Id { get; set; }
    public string Url { get; set; }
    public string Locale { get; set; }
    public string PostalCode { get; set; }
    public string Timezone { get; set; }
    public City City { get; set; }
    public State State { get; set; }
    public Country Country { get; set; }
    public Address Address { get; set; }
    public Location Location { get; set; }
}

public class City
{
    public string Name { get; set; }
}

public class State
{
    public string Name { get; set; }
}

public class Country
{
    public string Name { get; set; }
    public string CountryCode { get; set; }
}

public class Address
{
    public string Line1 { get; set; }
}

public class Location
{
    public double Longitude { get; set; }
    public double Latitude { get; set; }
}