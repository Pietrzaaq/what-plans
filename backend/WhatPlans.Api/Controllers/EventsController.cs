using MediatR;
using Microsoft.AspNetCore.Mvc;
using WhatPlans.Application.Events.Create;
using WhatPlans.Application.Events.Delete;
using WhatPlans.Application.Events.Get.All;
using WhatPlans.Application.Events.Get.ById;
using WhatPlans.Application.Events.Update;

namespace WhatPlans.Api.Controllers;

[Route("api/events")]
public class EventsController : BaseController
{
    public EventsController(IMediator mediator)
        : base(mediator) { }
    
    [HttpGet]
    public Task<IActionResult> GetEvents(GetEventsRequest request)
        => ExecuteAsync(request);
    
    [HttpGet("{id}")]
    public Task<IActionResult> GetEventById(GetEventByIdRequest request)
        => ExecuteAsync(request);
    
    [HttpPost]
    public Task<IActionResult> Create(CreateEventRequest request)
        => ExecuteAsync(request);
    
    [HttpPut("{id}")]
    public Task<IActionResult> UpdateEvent(UpdateEventRequest request)
        => ExecuteAsync(request);
    
    [HttpDelete("{id}")]
    public Task<IActionResult> DeleteEvent(DeleteEventRequest request)
        => ExecuteAsync(request);
}