using MediatR;
using Microsoft.AspNetCore.Mvc;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Update;

public class UpdateEventRequest : IRequest<Event>
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