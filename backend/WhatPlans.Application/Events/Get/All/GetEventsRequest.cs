using MediatR;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Get.All;

public class GetEventsRequest : IRequest<List<Event>>
{
}