using System;
using System.Collections.Generic;

namespace WebApi.ViewModels
{
    public class HomeTaskAssessmentVM
    {
        public string HomeTaskTitle { get; set; }

        public DateTime Date { get; set; }

        public int HomeTaskId { get; set; }

        public List<HomeTaskAssessmentStudentViewModel> homeTaskAssessmentStudents { get; set; } = new();
    }
    public class HomeTaskAssessmentStudentViewModel
    {
        public int HomeTaskAssessmentId { get; set; }

        public int StudentId { get; set; }

        public string StudentName { get; set; }

        public bool IsComplete { get; set; }

    }
}