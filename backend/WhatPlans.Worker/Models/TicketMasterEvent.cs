using Newtonsoft.Json;

namespace WhatPlans.Worker.Models;

public class ApiResponse
{
    [JsonProperty("_embedded")]
    public EmbeddedEventsData Embedded { get; set; }
}

public class EmbeddedEventsData
{
    [JsonProperty("events")]
    public List<TicketMasterEvent> Events { get; set; }
}

public class EmbeddedVenuesData
{
    [JsonProperty("venues")]
    public List<TicketMasterPlace> Venues { get; set; }
}

public class TicketMasterEvent
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string Id { get; set; }
    public string Url { get; set; }
    public string Locale { get; set; }
    public List<Image> Images { get; set; }
    public Sales Sales { get; set; }
    public EventDates Dates { get; set; }
    public List<Classification> Classifications { get; set; }
    public Promoter Promoter { get; set; }
    public List<PriceRange> PriceRanges { get; set; }
    public SeatMap SeatMap { get; set; }
    
    [JsonProperty("_embedded")]
    public EmbeddedVenuesData Embedded { get; set; }
}

public class Image
{
    public string Ratio { get; set; }
    public string Url { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public bool Fallback { get; set; }
}

public class Sales
{
    public PublicSales Public { get; set; }
}

public class PublicSales
{
    public DateTime StartDateTime { get; set; }
    public DateTime EndDateTime { get; set; }
    public bool StartTBD { get; set; }
    public bool StartTBA { get; set; }
}

public class EventDates
{
    public EventStart Start { get; set; }
    public string Timezone { get; set; }
    public EventStatus Status { get; set; }
    public bool SpanMultipleDays { get; set; }
}

public class EventStart
{
    public string LocalDate { get; set; }
    public string LocalTime { get; set; }
    public bool DateTBD { get; set; }
    public bool DateTBA { get; set; }
    public bool TimeTBA { get; set; }
    public bool NoSpecificTime { get; set; }
}

public class EventStatus
{
    public string Code { get; set; }
}

public class Classification
{
    public bool Primary { get; set; }
    public Segment Segment { get; set; }
    public Genre Genre { get; set; }
    public SubGenre SubGenre { get; set; }
    public bool Family { get; set; }
}

public class Segment
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class Genre
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class SubGenre
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class Promoter
{
    public string Id { get; set; }
    public string Name { get; set; }
}

public class PriceRange
{
    public string Type { get; set; }
    public string Currency { get; set; }
    public double Min { get; set; }
    public double Max { get; set; }
}

public class SeatMap
{
    public string StaticUrl { get; set; }
}