using System.Drawing;
using MongoDB.Bson;
using System.Drawing.Imaging;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace WhatPlans.Job.Services;

public class ImageService
{
    private const long MaxFileSizeBytes = 20 * 1024 * 1024; // 20 MB
    private const int MaxImageWidth = 10000;
    private const int MaxImageHeight = 10000;
    
    public async Task<WhatPlans.Domain.Entities.Image> SaveImageFromUrlAsync(string url, ObjectId relatedObjectId, string relatedObjectType, string description, ObjectId uploadedBy, string altText)
    {
        if (!IsValidUrl(url))
            throw new InvalidOperationException("Invalid or untrusted URL.");

        using var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("WhatPlans/0.0 (rambo4801@gmail.com)");
        var imageBytes = await httpClient.GetByteArrayAsync(url);

        if (imageBytes.Length > MaxFileSizeBytes)
            throw new InvalidOperationException("File size exceeds the allowed limit.");

        if (!IsSupportedImageFormat(imageBytes))
            throw new InvalidOperationException("Unsupported image format.");

        using var memoryStream = new MemoryStream(imageBytes);
        using var image = Image.FromStream(memoryStream);
        
        if (image.Width > MaxImageWidth || image.Height > MaxImageHeight)
            throw new InvalidOperationException($"Image dimensions exceed the allowed limit. Image Width: {image.Width}, Image Height: {image.Height}");
        
        // Resize image to fit within 800x600 resolution
        var resizedImageBytes = ResizeImage(imageBytes);
        using var resizedMemoryStream = new MemoryStream(resizedImageBytes);
        using var resizedImage = Image.FromStream(resizedMemoryStream);

        // Sanitize metadata
        using var sanitizedImage = new Bitmap(resizedImage);
        using var sanitizedStream = new MemoryStream();
        sanitizedImage.Save(sanitizedStream, ImageFormat.Png); // Convert to PNG
        resizedImageBytes = sanitizedStream.ToArray();

        var resolution = $"{resizedImage.Width}x{resizedImage.Height}";
        var format = resizedImage.RawFormat.ToString(); // Example: "Png", "Jpeg"
        var size = resizedImageBytes.Length;

        var sanitizedDescription = SanitizeInput(description);
        var sanitizedAltText = SanitizeInput(altText);

        var imageRecord = new WhatPlans.Domain.Entities.Image()
        {
            Id = ObjectId.GenerateNewId(),
            Url = url,
            BinaryData = resizedImageBytes,
            RelatedObjectId = relatedObjectId,
            RelatedObjectType = SanitizeInput(relatedObjectType),
            ImageType = format,
            Description = sanitizedDescription,
            UploadedBy = uploadedBy,
            UploadDate = DateTime.UtcNow,
            Resolution = resolution,
            Format = format,
            Size = size,
            AltText = sanitizedAltText
        };

        return imageRecord;
    }

    private bool IsValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult)
               && (uriResult.Scheme == Uri.UriSchemeHttps);
    }
    
    private bool IsSupportedImageFormat(byte[] imageBytes)
    {
        var supportedFormats = new[] { "image/jpeg", "image/png", "image/gif" };

        using var stream = new MemoryStream(imageBytes);
        using var image = Image.FromStream(stream);

        var mimeType = GetMimeType(image);
        return supportedFormats.Contains(mimeType);
    }

    private string GetMimeType(Image image)
    {
        if (ImageFormat.Jpeg.Equals(image.RawFormat))
            return "image/jpeg";
        if (ImageFormat.Png.Equals(image.RawFormat))
            return "image/png";
        if (ImageFormat.Gif.Equals(image.RawFormat))
            return "image/gif";
        return "unknown";
    }
    
    private string SanitizeInput(string input)
    {
        return System.Web.HttpUtility.HtmlEncode(input);
    }
    
    private static byte[] ResizeImage(byte[] imageData)
    {
        using var image = SixLabors.ImageSharp.Image.Load(imageData);

        // Resize while preserving aspect ratio
        image.Mutate(x => x.Resize(new ResizeOptions
        {
            Size = new SixLabors.ImageSharp.Size(800, 600),
            Mode = ResizeMode.Max
        }));

        using var outputStream = new MemoryStream();
        image.Save(outputStream, new JpegEncoder { Quality = 80 });
        return outputStream.ToArray();
    }
}