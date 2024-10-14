using MediatR;
using Microsoft.AspNetCore.Mvc;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Map.Cities.Get;

public class Request : IRequest<List<City>>
{
    [FromQuery(Name = "geohashes[]")]
    public List<string> Geohashes { get; set; }
}