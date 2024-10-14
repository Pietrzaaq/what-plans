using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get.ByIdWithEvents;

public class Request : IRequest<PlaceWithEvents>
{
    [FromRoute] 
    public ObjectId Id { get; set; }
}