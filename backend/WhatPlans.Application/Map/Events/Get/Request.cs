using MediatR;
using Microsoft.AspNetCore.Mvc;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Map.Events.Get;

public class Request : IRequest<List<PlaceWithEvents>>
{
    [FromQuery(Name = "startDate")]
    public DateTime? StartDate { get; set; }
    
    [FromQuery(Name = "endDate")]
    public DateTime? EndDate { get; set; }
    
    [FromQuery(Name = "geohashes[]")]
    public List<string> Geohashes { get; set; }
}