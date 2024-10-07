using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Update;

public class UpdateEventRequest : IRequest<Event>
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