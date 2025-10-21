using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace eUIT.API.Models
{
    [Table("refresh_tokens")]
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;

        [Required]
        public string Role { get; set; } = null!;

        [Required]
        public string Token { get; set; } = null!;

        [Required]
        public DateTime ExpiryDate { get; set; }
    }
}
