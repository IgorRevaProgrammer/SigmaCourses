using System;
using System.ComponentModel.DataAnnotations;

namespace WebApi.ViewModels
{
    public class HomeTaskVM
    {
        public int Id { get; set; }

        [Range(typeof(DateTime), "1/1/2022", "1/1/2122",
            ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime Date { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Range(1,100)]
        public int Number { get; set; }

        public int CourseId { get; set; }
    }
}
