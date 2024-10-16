﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Get.ByPlaceId;

public class Request : IRequest<List<Event>>
{
    [FromRoute]
    public ObjectId PlaceId { get; set; }
}