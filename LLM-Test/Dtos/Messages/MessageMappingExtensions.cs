using LLM_Test.Data.Entities;
using LLM_Test.Dtos.ImageAttachmented;

namespace LLM_Test.Dtos.Messages;

public static class MessageMappingExtensions 
{
    public static GetMessageDto ToGetDto(this Message message) 
    {
        return new GetMessageDto()
        {
            Id = message.Id,
            ImageAttachments = new List<GetImageAttachmentDto>(),
            Role = message.Role,
            Text = message.Text,
            Thoughts = message.Thoughts
        };
    }

    public static ICollection<GetMessageDto> ToGetDtoList(this ICollection<Message> messages) 
    {
        return messages.Select(message => message.ToGetDto()).ToList();

    }
}
