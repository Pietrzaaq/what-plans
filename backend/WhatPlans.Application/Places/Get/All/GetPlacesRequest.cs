using MediatR;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get.All;

public class GetPlacesRequest : IRequest<List<Place>>
{
}