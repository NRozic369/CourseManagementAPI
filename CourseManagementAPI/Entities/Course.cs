using System;
using System.Collections.Generic;

namespace CourseManagementAPI.Entities
{
    public partial class Course
    {
        public Course()
        {
            Attendees = new HashSet<Attendee>();
        }

        public int Id { get; set; }
        public string CourseTitle { get; set; } = null!;
        public string? CourseDescription { get; set; }
        public DateTime CourseStartDateTime { get; set; }
        public string CourseTeacher { get; set; } = null!;
        public string CourseTeacherEmail { get; set; } = null!;
        public string EditDeleteCoursePin { get; set; } = null!;
        public int MaxNumberOfAtendees { get; set; }

        public virtual ICollection<Attendee> Attendees { get; set; }
    }
}
