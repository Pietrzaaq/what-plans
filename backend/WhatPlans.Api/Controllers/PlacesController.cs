using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WhatPlans.Api.Controllers;

[Route("api/places")]
public class PlacesController : BaseController
{
    public PlacesController(IMediator mediator)
        : base(mediator) { }
    
    [HttpGet]
    public Task<IActionResult> GetPlaces(Application.Places.Get.All.Request request)
        => ExecuteAsync(request);
    
    [HttpGet("events")]
    public Task<IActionResult> GetPlacesWithEvents(Application.Places.Get.AllWithEvents.Request request)
        => ExecuteAsync(request);
    
    [HttpGet("{id}/events")]
    public Task<IActionResult> GetPlaceByIdWithEvents(Application.Places.Get.ByIdWithEvents.Request request)
        => ExecuteAsync(request);
    
    [HttpGet("{id}")]
    public Task<IActionResult> GetPlaceById(Application.Places.Get.ById.Request request)
        => ExecuteAsync(request);
    [HttpPost]
    public Task<IActionResult> Create(Application.Places.Create.Request request)
        => ExecuteAsync(request);
    
    [HttpPut("{id}")]
    public Task<IActionResult> UpdatePlace(Application.Places.Update.Request request)
        => ExecuteAsync(request);
    
    [HttpDelete("{id}")]
    public Task<IActionResult> DeletePlace(Application.Places.Delete.Request request)
        => ExecuteAsync(request);
}