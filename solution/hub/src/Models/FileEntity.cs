using System.ComponentModel.DataAnnotations;

namespace hub.Models;

public class FileEntity
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string FileName { get; set; }

    [Required]
    public DateTime UploadDate { get; set; }

    [Required]
    public Guid UserId { get; set; }
}