using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Get.ById;

public class Request : IRequest<Event>
{
    [FromRoute]
    public ObjectId Id { get; set; }
}