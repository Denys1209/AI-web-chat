using Chat;
using LLM_Test.Constants;
using LLM_Test.Data.Shared;
using System.ComponentModel.DataAnnotations;

namespace LLM_Test.Data.Entities;

public class Message : ModelWithTimeStamp
{
    public required string Text { get; set; }

    public string Thoughts { get; set; } = string.Empty;

    public required Roles Role { get; set; }


    public virtual ICollection<ImageAttached> ImageAttacheds { get; set; } = new List<ImageAttached>();

    public virtual required Thread Thread { get; set; }
}
