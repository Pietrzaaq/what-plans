using MediatR;
using Microsoft.AspNetCore.Mvc;
using WhatPlans.Application.Places.Get;

namespace WhatPlans.Api.Controllers;

public class PlacesController : BaseController
{
    public PlacesController(IMediator mediator)
        : base(mediator) { }
    
    [HttpGet("places")]
    public Task<IActionResult> GetPlaces(GetPlacesQuery query)
        => ExecuteAsync(query);
}