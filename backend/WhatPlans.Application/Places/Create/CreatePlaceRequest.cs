﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using WhatPlans.Domain.Common;
using WhatPlans.Domain.Entities;
using WhatPlans.Domain.Enums;

namespace WhatPlans.Application.Places.Create;

public class CreatePlaceRequest : IRequest<Place>
{
    [FromBody]
    public Data Body { get; set; }

    public class Data
    {
        public string Name { get; set; }
        public PlaceTypes Type { get; set; }
        public string CreatorId { get; set; }
        public Location Location { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Mail { get; set; }
        public string Polygon { get; set; }
        public int? Capacity { get; set; }
    }
}