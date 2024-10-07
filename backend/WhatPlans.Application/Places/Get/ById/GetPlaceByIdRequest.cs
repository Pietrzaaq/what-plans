using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get.ById;

public class GetPlaceByIdRequest : IRequest<Place>
{
    [FromRoute]
    public ObjectId Id { get; set; }
}