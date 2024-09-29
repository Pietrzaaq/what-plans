using MongoDB.Bson;

namespace WhatPlans.Domain.Entities;

public class Image
{
    public ObjectId ImageId { get; set; }

    public byte[] BinaryData { get; set; }  

    public ObjectId RelatedObjectId { get; set; } 

    public string RelatedObjectType { get; set; } 

    public string ImageType { get; set; } 

    public string Description { get; set; }  

    public ObjectId UploadedBy { get; set; }

    public DateTime UploadDate { get; set; } 

    public string Resolution { get; set; }  

    public string Format { get; set; } 

    public long Size { get; set; } 

    public string AltText { get; set; } 
}