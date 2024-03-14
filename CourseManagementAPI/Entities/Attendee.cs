using System;
using System.Collections.Generic;

namespace CourseManagementAPI.Entities
{
    public partial class Attendee
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public int CourseId { get; set; }

        public virtual Course Course { get; set; } = null!;
    }
}
