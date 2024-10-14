using MediatR;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Cities.Get;

public class Request : IRequest<List<City>>
{
    
}