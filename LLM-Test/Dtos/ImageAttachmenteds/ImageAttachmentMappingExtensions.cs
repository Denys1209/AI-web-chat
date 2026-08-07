using LLM_Test.Data.Entities;
using LLM_Test.Dtos.ImageAttachmented;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography.X509Certificates;

namespace LLM_Test.Dtos.ImageAttachmenteds;

public static class ImageAttachmentMappingExtensions
{

    public static GetImageAttachmentDto ToGetDto(this ImageAttached image) 
    {
        return new GetImageAttachmentDto
        {
            Id = image.Id,
            Url = $"/images/{image.Path}",
            MimeType = image.Type
        };
    }

    public static ICollection<GetImageAttachmentDto> ToGetDtoList(this ICollection<ImageAttached> images) 
    {
        return images.Select(image => image.ToGetDto()).ToList();
    }

}
