
using LLM_Test.Data.Entities;
using LLM_Test.Dtos.ImageAttachments;

namespace LLM_Test.Dtos.Messages;

public static class MessageMappingExtension
{
    public static GetMessageDto ToGetDto(this Message message) 
    {

        return new GetMessageDto
        {
            Id = message.Id,
            ImageAttachmentDtos = message.ImageAttacheds.ToGetDtoList(),
            Role = message.Role,
            Text = message.Text,
            Thoughts = message.Thoughts,
        };

    }

    public static ICollection<GetMessageDto> ToGetDtoList(this ICollection<Message> messages) 
    {
        return messages.Select(message => message.ToGetDto()).ToList();
    }

}
