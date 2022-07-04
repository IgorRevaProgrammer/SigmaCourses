using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApi.ViewModels
{
    public class CourseVM
    {
        [Required]
        public string Name { get; set; }

        public int Id { get; set; }
        [Required]
        [Range(typeof(DateTime), "1/1/2022", "12/12/2122",
            ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime StartDate { get; set; }
        [Required]
        [Range(typeof(DateTime), "1/1/2022", "12/12/2122",
            ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public DateTime EndDate { get; set; }
        [Required]
        [Range(typeof(int), "1","100", 
            ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public int PassCredits { get; set; }

        public virtual List<HomeTaskVM> HomeTasks { get; set; } = new List<HomeTaskVM>();
    }
}
