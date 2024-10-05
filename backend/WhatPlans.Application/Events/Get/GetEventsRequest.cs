using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Get;

public class GetEventsRequest : IRequest<List<EventWithLocation>>
{
    [FromQuery]
    public ObjectId? PlaceId { get; set; }
}