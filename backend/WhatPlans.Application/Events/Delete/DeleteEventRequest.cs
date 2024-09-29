using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WhatPlans.Application.Events.Delete;

public class DeleteEventRequest : IRequest
{
    [FromRoute]
    public string Id { get; set; }
}