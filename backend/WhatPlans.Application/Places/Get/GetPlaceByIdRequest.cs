using MediatR;
using WhatPlans.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;


namespace WhatPlans.Application.Places.Get;

public class GetPlaceByIdRequest : IRequest<Place>
{
    [FromRoute]
    public ObjectId Id { get; set; }
}