using LLM_Test.Constants;
using LLM_Test.Data.Shared;
using System.ComponentModel.DataAnnotations;

namespace LLM_Test.Data.Entities;

public class ImageAttached : Model
{
    [MaxLength(NumberConstants.MaxLengthPath)]
    public required string Path { get; set; } 


    [MaxLength(NumberConstants.MaxLengthImageType)]
    public required string Type { get; set; } 

    public virtual required Message Message { get; set; }
}
