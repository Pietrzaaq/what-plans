using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace WhatPlans.Application.Places.Delete;

public class Request : IRequest
{
    [FromRoute]
    public ObjectId Id { get; set; }
}