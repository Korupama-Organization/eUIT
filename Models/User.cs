using System;
using System.Collections.Generic;

namespace eUIT.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        
        // Quan hệ 1-n với RefreshToken
        public List<RefreshToken> RefreshTokens { get; set; }
    }
}