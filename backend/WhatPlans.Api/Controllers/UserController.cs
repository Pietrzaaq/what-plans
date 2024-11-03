using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;
using WhatPlans.Domain.Exceptions;

namespace WhatPlans.Api.Controllers;

[Route("api/users")]
public class UserController : BaseController
{
    private readonly IMongoContext _mongoContext;
    private readonly IUserManager _userManager;
    private readonly IJwtService _jwtService;
    private readonly IDateTimeProvider _dateTimeProvider;

    public UserController(IMediator mediator, IMongoContext mongoContext, IUserManager userManager, IJwtService jwtService, IDateTimeProvider dateTimeProvider)
        : base(mediator)
    {
        _mongoContext = mongoContext;
        _userManager = userManager;
        _jwtService = jwtService;
        _dateTimeProvider = dateTimeProvider;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(Application.Users.Create.Request request)
    {
        var user = new User
        {
            UserName = request.Body.Username,
            FirstName = request.Body.FirstName,
            LastName = request.Body.LastName,
            Email = request.Body.Email,
            BirthDate = DateOnly.FromDateTime(request.Body.BirthDate),
            IsAdmin = false,
            Culture = request.Body.Culture,
            RegisterDate = _dateTimeProvider.UtcNow
        };

        if (await _userManager.FindByEmailAsync(request.Body.Email) is not null)
            throw new EmailAlreadyInUseException();
        
        if (await _userManager.FindByUsernameAsync(request.Body.Username) is not null)
            throw new UsernameAlreadyInUseException();

        var result = await _userManager.Register(user, request.Body.Password);

        if (!result.Succeeded)
        {
            throw new IdentityException(result.Errors);
        }

        return Created();
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login(Application.Users.Login.Request request)
    {
        var user = await _mongoContext.Users.Find(x => x.Email == request.Body.Email).FirstOrDefaultAsync();
        if (user == null)
        {
            throw new CredentialsException();
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, request.Body.Password);
        if (!isPasswordValid)
        {
            throw new CredentialsException();
        }
        
        var token = _jwtService.GenerateToken(user);
        return Ok(token);
    }
    
    [Authorize]
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        if (string.IsNullOrWhiteSpace(HttpContext.User.Identity?.Name))
        {
            return NotFound();
        }
        
        var userId = ObjectId.Parse(HttpContext.User.Identity.Name);
        var user = await _mongoContext.Users.Find(u => u.Id == userId).FirstAsync();

        var response = new UserDto()
        {
            Username = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDate = user.BirthDate,
            RegisterDate = user.RegisterDate,
            Culture = user.Culture,
            LastVisitDate = user.LastVisitDate,
            IsAdmin = user.IsAdmin,
            IsOrganizer = user.IsOrganizer,
            AvatarUrl = user.AvatarUrl
        };

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Application.Users.Get.ById.Request request)
    {
        var user = await _mongoContext.Users.Find(u => u.Id == request.Id).FirstAsync();

        var userDto = new UserLightDto()
        {
            Username = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            IsAdmin = user.IsAdmin,
            IsOrganizer = user.IsOrganizer,
            AvatarUrl = user.AvatarUrl,
        };
        
        return Ok(userDto);
    }
}