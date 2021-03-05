﻿using System.ComponentModel.DataAnnotations;

namespace RtuItLab.Infrastructure.Models.Identity
{
    public class AuthenticateRequest
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }
    }
}