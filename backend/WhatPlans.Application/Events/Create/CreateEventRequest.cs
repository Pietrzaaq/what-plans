using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WhatPlans.Domain.Common;
using WhatPlans.Domain.Entities;
using WhatPlans.Domain.Enums;

namespace WhatPlans.Application.Events.Create;

public class CreateEventRequest : IRequest<Event>
{
    [FromBody]
    public Data Body { get; set; }

    public class Data
    {
        public string Name { get; set; }
        public EventTypes Type { get; set; }
        public ObjectId? PlaceId { get; set; }
        public string CreatorId { get; set; }
        public string Url { get; set; }
        public Location Location { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public List<string> ImageUrls { get; set; }
    }
}