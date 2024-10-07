using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Get.All;

public class GetEventsRequest : IRequest<List<EventWithLocation>>
{
}