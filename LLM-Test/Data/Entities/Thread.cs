using LLM_Test.Constants;
using LLM_Test.Data.Shared;
using System.ComponentModel.DataAnnotations;

namespace LLM_Test.Data.Entities;

public class Thread : ModelWithTimeStamp
{
    [MaxLength(NumberConstants.MaxLengthTheadName)]
    public required string Name { get; set; }

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    public virtual required User User { get; set; }
}
