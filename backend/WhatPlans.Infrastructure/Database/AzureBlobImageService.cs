using Azure.Storage.Blobs;
using MongoDB.Bson;
using MongoDB.Driver;
using WhatPlans.Application.Interfaces;
using WhatPlans.Domain.Entities;

namespace WhatPlans.Infrastructure.Database;

public class AzureBlobImageService : IImageService
{
    private readonly BlobContainerClient _containerClient;
    private readonly IMongoContext _mongoContext;

    public AzureBlobImageService(AzureBlobStorageSettings azureBlobStorageSettings, IMongoContext mongoContext)
    {
        _mongoContext = mongoContext;
        
        var blobServiceClient = new BlobServiceClient(azureBlobStorageSettings.ConnectionString);
        _containerClient = blobServiceClient.GetBlobContainerClient(azureBlobStorageSettings.ContainerName);
        _containerClient.CreateIfNotExists();
    }

    public async Task<ObjectId> UploadImageAsync(Image image)
    {
        var blobName = $"{Guid.NewGuid()}.{image.Format}";
        var blobClient = _containerClient.GetBlobClient(blobName);

        using var stream = new MemoryStream(image.BinaryData);
        await blobClient.UploadAsync(stream, true);

        image.Url = blobClient.Uri.ToString();
        image.BinaryData = null;
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
        
        var blobClient = _containerClient.GetBlobClient(imageId.ToString());
        await blobClient.DeleteIfExistsAsync();
    }
}