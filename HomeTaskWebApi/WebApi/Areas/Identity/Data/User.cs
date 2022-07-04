using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace WebApi.Areas.Identity.Data
{
    public class User : IdentityUser
    {
        [Required]
        [PersonalData]
        public string Name { get; set; } = null!;

        [Required]
        [PersonalData]
        public DateTime DateOfBirth { get; set; }
    }
}
