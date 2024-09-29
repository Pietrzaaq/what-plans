using MediatR;
using WhatPlans.Domain.Entities;
using Microsoft.AspNetCore.Mvc;


namespace WhatPlans.Application.Places.Get;

public class GetPlaceByIdRequest : IRequest<Place>
{
    [FromRoute]
    public string Id { get; set; }
}