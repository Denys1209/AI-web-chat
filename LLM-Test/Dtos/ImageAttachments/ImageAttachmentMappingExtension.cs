using LLM_Test.Data.Entities;
using LLM_Test.Dtos.Users;

namespace LLM_Test.Dtos.ImageAttachments;

public static class ImageAttachmentMappingExtension
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
