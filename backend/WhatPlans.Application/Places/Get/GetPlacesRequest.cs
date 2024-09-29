using MediatR;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Get;

public class GetPlacesRequest : IRequest<List<Place>>
{
}