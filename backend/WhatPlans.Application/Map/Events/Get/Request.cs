using MediatR;
using Microsoft.AspNetCore.Mvc;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Map.Events.Get;

public class Request : IRequest<List<PlaceWithEvents>>
{
    [FromQuery(Name = "geohashes[]")]
    public List<string> Geohashes { get; set; }
}