using MediatR;
using Microsoft.AspNetCore.Mvc;
using WhatPlans.Application.Places.Create;
using WhatPlans.Application.Places.Delete;
using WhatPlans.Application.Places.Get.All;
using WhatPlans.Application.Places.Get.ById;
using WhatPlans.Application.Places.Get.ByIdWithEvents;
using WhatPlans.Application.Places.Update;

namespace WhatPlans.Api.Controllers;

[Route("api/places")]
public class PlacesController : BaseController
{
    public PlacesController(IMediator mediator)
        : base(mediator) { }
    
    [HttpGet]
    public Task<IActionResult> GetPlaces(GetPlacesRequest request)
        => ExecuteAsync(request);
    
    [HttpGet("{id}/events")]
    public Task<IActionResult> GetPlaceByIdWithEvents(GetPlaceWithEventsRequest request)
        => ExecuteAsync(request);
    
    [HttpGet("{id}")]
    public Task<IActionResult> GetPlaceById(GetPlaceByIdRequest request)
        => ExecuteAsync(request);
    [HttpPost]
    public Task<IActionResult> Create(CreatePlaceRequest request)
        => ExecuteAsync(request);
    
    [HttpPut("{id}")]
    public Task<IActionResult> UpdatePlace(UpdatePlaceRequest request)
        => ExecuteAsync(request);
    
    [HttpDelete("{id}")]
    public Task<IActionResult> DeletePlace(DeletePlaceRequest request)
        => ExecuteAsync(request);
}