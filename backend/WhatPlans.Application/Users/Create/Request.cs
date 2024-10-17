using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WhatPlans.Application.Users.Create;

public class Request : IRequest<IdentityResult>
{
    [FromBody]
    public Data Body { get; set; }

    public class Data
    {
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public DateOnly BirthDate { get; set; }
        public string Password { get; set; }
        public string Culture { get; set; }
    }
}