using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.ViewModels
{
    public class StudentVM
    {
        [Required]
        public string Name { get; set; }

        public int Id { get; set; }

        [Required]
        [Range(typeof(DateTime), "1/1/1900", "1/1/2122",
            ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        [Required]
        public string Email { get; set; }

        public string GitHubLink { get; set; }

        public string Notes { get; set; }
    }
}
