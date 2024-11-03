using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Users.Get.ById;

public class Request : IRequest<UserLightDto>
{
    [FromRoute]
    public ObjectId Id { get; set; }
}