using MediatR;
using Microsoft.AspNetCore.Mvc;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get;

public class GetPlacesWithEventsRequest : IRequest<List<PlaceWithEvents>>
{
    [FromQuery] public bool IncludeEvents { get; set; }
}