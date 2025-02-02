using System.ComponentModel.DataAnnotations;

namespace hub.Models;

public class ProcessFile
{
    [Required]
    public string filename { get; set; }
    [Required]
    public string fileId { get; set; }
}