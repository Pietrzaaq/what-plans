using System.Net;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WhatPlans.Api.Controllers;

[Controller]
[Route("api")]
public class BaseController : ControllerBase
{
    private readonly IMediator _mediator;

    public BaseController(IMediator mediator)
    {
        _mediator = mediator;
    }
    
    protected async Task<IActionResult> ExecuteAsync<TRequest>(TRequest request)
    {
        try
        {
            var result = await _mediator.Send(request);

            if (request is IRequest)
                return NoContent();

            return Ok(result);
        }
        catch (ValidationException e)
        {
            return BadRequest(e.Errors);
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, e.Message);
        }
        finally
        {
        }
    }
}