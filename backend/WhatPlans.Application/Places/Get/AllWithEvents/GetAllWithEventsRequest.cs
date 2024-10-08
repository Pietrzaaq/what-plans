using MediatR;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get.AllWithEvents;

public class GetAllWithEventsRequest : IRequest<List<PlaceWithEvents>>
{
}