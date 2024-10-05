using MediatR;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get;

public class GetPlacesRequest : IRequest<List<PlaceWithLocation>>
{
}