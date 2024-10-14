using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Places.Update;

public class Request : IRequest<Place>
{
    [FromRoute]
    public ObjectId Id { get; set; }
    
    [FromBody]
    public Data Body { get; set; }

    public class Data
    {
        public string Name { get; set; }
    }
}