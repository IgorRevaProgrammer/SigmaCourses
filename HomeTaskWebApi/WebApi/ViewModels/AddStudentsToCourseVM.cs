using System;
using System.Collections.Generic;

namespace WebApi.ViewModels
{
    public class AddStudentsToCourseVM
    {
        public string Name { get; set; }

        public int Id { get; set; }

        public virtual List<AssignmentStudentVM> students { get; set; } = new List<AssignmentStudentVM>();
    }
    public class AssignmentStudentVM
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsAssigned { get; set; }
    }
}
