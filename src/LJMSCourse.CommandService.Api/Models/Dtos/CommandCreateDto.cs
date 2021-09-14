using System.ComponentModel.DataAnnotations;

namespace LJMSCourse.CommandService.Api.Models.Dtos
{
    public class CommandCreateDto
    {
        [Required] public string HowTo { get; set; }

        [Required] public string CommandLine { get; set; }
    }
}