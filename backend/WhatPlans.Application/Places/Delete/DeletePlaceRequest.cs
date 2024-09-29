using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WhatPlans.Application.Places.Delete;

public class DeletePlaceRequest : IRequest
{
    [FromRoute]
    public string Id { get; set; }
}