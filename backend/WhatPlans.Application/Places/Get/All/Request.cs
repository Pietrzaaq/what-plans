using MediatR;
using Microsoft.AspNetCore.Mvc;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get.All;

public class Request : IRequest<List<Place>>
{
    [FromQuery(Name = "geohashes[]")]
    public List<string> Geohashes { get; set; }
}