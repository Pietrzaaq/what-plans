using MediatR;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get.All;

public class Request : IRequest<List<Place>>
{
}