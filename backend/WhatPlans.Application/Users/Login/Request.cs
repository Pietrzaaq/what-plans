using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WhatPlans.Application.Users.Login;

public class Request : IRequest<string>
{
    [FromBody]
    public Data Body { get; set; }

    public class Data
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}