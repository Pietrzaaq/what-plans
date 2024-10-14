using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WhatPlans.Api.Controllers;

[Route("api/map")]
public class MapController : BaseController
{
    public MapController(IMediator mediator)
        : base(mediator) { }
    
    [HttpGet("places")]
    public Task<IActionResult> GetPlaces(Application.Map.Places.Get.Request request)
        => ExecuteAsync(request);
    
    [HttpGet("events")]
    public Task<IActionResult> GetEvents(Application.Map.Events.Get.Request request)
        => ExecuteAsync(request);
    
    [HttpGet("cities")]
    public Task<IActionResult> GetCities(Application.Map.Cities.Get.Request request)
        => ExecuteAsync(request);
}