using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace WhatPlans.Application.Images.Get;

public class Request : IRequest
{
    [FromRoute]
    public ObjectId Id { get; set; }
}