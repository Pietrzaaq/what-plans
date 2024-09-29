using MediatR;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Get;

public class GetEventsRequest : IRequest<List<Event>>
{
}