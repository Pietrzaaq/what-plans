using MongoDB.Bson;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Application.Interfaces;

public interface IImageService
{
    Task<ObjectId> UploadImageAsync(Image image);
    Task<Image> GetImageAsync(ObjectId imageId);
    Task DeleteImageAsync(ObjectId imageId);
}