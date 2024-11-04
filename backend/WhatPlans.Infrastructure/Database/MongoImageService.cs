using MongoDB.Bson;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Infrastructure.Database;

public class MongoImageService : IImageService
{
    private readonly IMongoContext _mongoContext;

    public MongoImageService(IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
    }

    public async Task<ObjectId> UploadImageAsync(Image image)
    {
        await _mongoContext.Images.InsertOneAsync(image);
        return image.Id;
    }

    public async Task<Image> GetImageAsync(ObjectId imageId)
    {
        var image = await _mongoContext.Images.Find(i => i.Id == imageId).FirstOrDefaultAsync();
        return image;
    }

    public async Task DeleteImageAsync(ObjectId imageId)
    { 
        await _mongoContext.Images.DeleteOneAsync(i => i.Id == imageId);
    }
}