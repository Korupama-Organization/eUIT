using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace eUIT.API.DTOs;

public class UpdateAvatarDto
{
    [Required]
    public IFormFile AvatarFile { get; set; } = default!;
}
