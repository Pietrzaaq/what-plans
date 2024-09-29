using MediatR;
using Microsoft.AspNetCore.Mvc;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Events.Create;

public class CreateEventRequest : IRequest<Event>
{
    [FromBody]
    public Data Event { get; set; }

    public class Data
    {
        public string Name { get; set; }
    }
}