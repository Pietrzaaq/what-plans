using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WhatPlans.Api.Controllers;

[Route("api/events")]
public class EventsController : BaseController
{
    public EventsController(IMediator mediator)
        : base(mediator) { }
    
    [HttpGet]
    public Task<IActionResult> GetEvents(Application.Events.Get.All.Request request)
        => ExecuteAsync(request);
    
    [HttpGet("places/{placeId}")]
    public Task<IActionResult> GetEventsByPlaceId(Application.Events.Get.ByPlaceId.Request request)
        => ExecuteAsync(request);
    
    [HttpGet("{id}")]
    public Task<IActionResult> GetEventById(Application.Events.Get.ById.Request request)
        => ExecuteAsync(request);
    
    [HttpPost]
    public Task<IActionResult> Create(Application.Events.Create.Request request)
        => ExecuteAsync(request);
    
    [HttpPut("{id}")]
    public Task<IActionResult> UpdateEvent(Application.Events.Update.Request request)
        => ExecuteAsync(request);
    
    [HttpDelete("{id}")]
    public Task<IActionResult> DeleteEvent(Application.Events.Delete.Request request)
        => ExecuteAsync(request);
}