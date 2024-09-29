using MediatR;
using WhatPlans.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;


namespace WhatPlans.Application.Events.Get;

public class GetEventByIdRequest : IRequest<Event>
{
    [FromRoute]
    public ObjectId Id { get; set; }
}