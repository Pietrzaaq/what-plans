using MediatR;
using Microsoft.AspNetCore.Mvc;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Update;

public class UpdatePlaceRequest : IRequest<Place>
{
    [FromRoute]
    public string Id { get; set; }
    
    [FromBody]
    public Data Body { get; set; }

    public class Data
    {
        public string Name { get; set; }
    }
}