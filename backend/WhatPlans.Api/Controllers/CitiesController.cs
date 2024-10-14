using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WhatPlans.Api.Controllers;

[Route("api/cities")]
public class CitiesController : BaseController
{
    public CitiesController(IMediator mediator)
        : base(mediator) { }
    
    [HttpGet]
    public Task<IActionResult> GetAll(Application.Cities.Get.Request request)
        => ExecuteAsync(request);
}