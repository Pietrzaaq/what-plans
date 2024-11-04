using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WhatPlans.Application.Images.Create;

public class Request : IRequest
{
    [FromForm]
    public string RelatedObjectType { get; set; }
    [FromForm]
    public string RelatedObjectId { get; set; }
    [FromForm]
    public IFormFile File { get; set; }
}