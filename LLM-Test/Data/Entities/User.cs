using LLM_Test.Constants;
using LLM_Test.Data.Shared;
using System.ComponentModel.DataAnnotations;

namespace LLM_Test.Data.Entities;

public class User : Model
{
    [MaxLength(NumberConstants.MaxLengthDisplayedName)]
    public required string DisplayedName { get; set; }

    [EmailAddress]
    public required string Gmail { get; set; }

    public required string PasswordHash { get; set; }

    public virtual ICollection<Thread> Threads { get; set; } = new List<Thread>();
}
