using MediatR;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get.AllWithEvents;

public class Request : IRequest<List<PlaceWithEvents>>
{
}