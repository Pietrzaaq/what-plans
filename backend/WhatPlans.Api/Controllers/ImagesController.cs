using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using WhatPlans.Application.Images.Create;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Api.Controllers;

[Authorize]
[Route("api/images")]
public class ImagesController : BaseController
{
    private static readonly string[] AllowedExtensions = [".jpg", ".jpeg", ".png", ".gif"];
    private readonly ILogger<ImagesController> _logger;
    private readonly IDateTimeProvider _dateTimeProvider;
    private readonly ICurrentUserProvider _currentUserProvider;
    private readonly IImageService _imageService;

    public ImagesController(IMediator mediator, IDateTimeProvider dateTimeProvider, ICurrentUserProvider currentUserProvider, IImageService imageService, ILogger<ImagesController> logger)
        : base(mediator)
    {
        _dateTimeProvider = dateTimeProvider;
        _currentUserProvider = currentUserProvider;
        _imageService = imageService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> UploadImage(Request request)
    {
        var user = await _currentUserProvider.GetUser();
        if (user is null)
        {
            return BadRequest("Uploading user is not found");
        }

        var file = request.File;
        if (file == null || file.Length == 0)
        {
            return BadRequest("No file was uploaded or the file is empty.");
        }
        
        var fileExtension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (!AllowedExtensions.Contains(fileExtension))
        {
            return BadRequest("Invalid file type. Only JPG, JPEG, PNG, and GIF files are allowed.");
        }
        
        const long maxFileSize = 5 * 1024 * 1024;
        if (file.Length > maxFileSize)
        {
            return BadRequest("File size exceeds the 5 MB limit.");
        }
        
        byte[] fileData;
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            fileData = memoryStream.ToArray();
        }

        var relatedObjectType = request.RelatedObjectType;
        var relatedObjectId = ObjectId.Parse(request.RelatedObjectId);
        
        var image = new Image
        {
            Id = ObjectId.GenerateNewId(),
            BinaryData = fileData,
            Format = fileExtension.TrimStart('.'),
            RelatedObjectType = relatedObjectType,
            RelatedObjectId = relatedObjectId,
            Size = file.Length,
            UploadDate = _dateTimeProvider.UtcNow,
            AltText = file.FileName,
            UploadedBy = user.Id,
            Description = "Uploaded image"
        };
        
        try
        {
            await _imageService.UploadImageAsync(image);
            return Ok(image.Id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error uploading image");
            return StatusCode(500, "An error occurred while uploading the image");
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetImage(Application.Images.Get.Request request)
    {
        var image = await _imageService.GetImageAsync(request.Id);
        if (image is null)
            return NotFound();

        var response = new ImageDto()
        {
            Id = image.Id,
            Url = image.Url,
            Description = image.Description,
            AltText = image.AltText,
        };
        
        return Ok(response);
    }
    
    [HttpGet("{id}/binary")]
    public async Task<IActionResult> GetImageBinary(Application.Images.Get.Request request)
    {
        var image = await _imageService.GetImageAsync(request.Id);
        if (image == null || image.BinaryData == null)
        {
            return NotFound();
        }

        return File(image.BinaryData, "image/png");
    }
}