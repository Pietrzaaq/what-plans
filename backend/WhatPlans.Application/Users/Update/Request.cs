using MediatR;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace WhatPlans.Application.Users.Update;

public class Request : IRequest
{
    [FromBody]
    public Data Body { get; set; }

    public class Data
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public string Culture { get; set; }
        public string AvatarId { get; set; }
    }
}