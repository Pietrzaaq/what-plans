using MediatR;
using Microsoft.AspNetCore.Mvc;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Map.Places.Get;

public class Request : IRequest<List<Place>>
{
    [FromQuery]
    public string Geohash { get; set; }
    
    [FromQuery]
    public double North { get; set; }    
    
    [FromQuery]
    public double South { get; set; }    
    
    [FromQuery]
    public double East { get; set; }    
    
    [FromQuery]
    public double West { get; set; }
}