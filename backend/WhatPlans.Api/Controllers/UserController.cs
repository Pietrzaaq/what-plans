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
    private readonly ICurrentUserProvider _currentUserProvider;

    public UserController(IMediator mediator, IMongoContext mongoContext, IUserManager userManager, IJwtService jwtService, IDateTimeProvider dateTimeProvider, ICurrentUserProvider currentUserProvider)
        : base(mediator)
    {
        _mongoContext = mongoContext;
        _userManager = userManager;
        _jwtService = jwtService;
        _dateTimeProvider = dateTimeProvider;
        _currentUserProvider = currentUserProvider;
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
        var user = await _currentUserProvider.GetUser();
        if (user is null)
        {
            return NotFound();
        }

        var response = new UserDto()
        {
            Id = user.Id,
            Username = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDate = user.BirthDate,
            RegisterDate = user.RegisterDate,
            Culture = user.Culture,
            LastVisitDate = user.LastVisitDate,
            IsAdmin = user.IsAdmin,
            IsOrganizer = user.IsOrganizer,
            AvatarId = user.AvatarId
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
            AvatarId = user.AvatarId,
        };
        
        return Ok(userDto);
    }
    
    [HttpPatch("me")]
    public async Task<IActionResult> UpdateUser(Application.Users.Update.Request request)
    {
        var user = await _currentUserProvider.GetUser();
        if (user is null)
            return NotFound();
        
        var filter = Builders<User>.Filter.Eq(p => p.Id, user.Id);
        ObjectId.TryParse(request.Body.AvatarId, out var avatarId);
        
        var update = Builders<User>.Update
            .Set(p => p.FirstName, request.Body.FirstName)
            .Set(p => p.LastName, request.Body.LastName)
            .Set(p => p.BirthDate, DateOnly.FromDateTime(request.Body.BirthDate))
            .Set(p => p.Culture, request.Body.Culture)
            .Set(p => p.AvatarId, ObjectId.Empty == avatarId ? null : avatarId);
        
        await _mongoContext.Users.UpdateOneAsync(filter, update);

        var response = new UserDto()
        {
            Id = user.Id,
            Username = user.UserName,
            FirstName = user.FirstName,
            LastName = user.LastName,
            BirthDate = user.BirthDate,
            RegisterDate = user.RegisterDate,
            Culture = user.Culture,
            LastVisitDate = user.LastVisitDate,
            IsAdmin = user.IsAdmin,
            IsOrganizer = user.IsOrganizer,
            AvatarId = user.AvatarId
        };
        
        return Ok(response);
    }
}