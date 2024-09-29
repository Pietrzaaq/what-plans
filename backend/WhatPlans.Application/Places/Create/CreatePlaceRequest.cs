using MediatR;
using Microsoft.AspNetCore.Mvc;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Create;

public class CreatePlaceRequest : IRequest<Place>
{
    [FromBody]
    public Data Place { get; set; }

    public class Data
    {
        public string Name { get; set; }
    }
}