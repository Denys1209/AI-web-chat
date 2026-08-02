using Chat;
using LLM_Test.Constants;
using LLM_Test.Data.Shared;
using System.ComponentModel.DataAnnotations;

namespace LLM_Test.Data.Entities;

public class Message : ModelWithTimeStamp
{
    [MaxLength(NumberConstants.MaxLengthText)]
    public required string Text { get; set; }

    public required Roles Role { get; set; }


    public virtual ICollection<ImageAttached> ImageAttacheds { get; set; } = new List<ImageAttached>();

    public virtual required Thread Thread { get; set; }
}
