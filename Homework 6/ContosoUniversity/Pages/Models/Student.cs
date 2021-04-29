using System;
using System.Collections.Generic;

namespace ContosoUniversity.Models
{
    public class Student
    {
        public int ID { get; set; }
        public string LastName { get; set; }
        public string FirstMidName { get; set; }
        public DateTime EnrollmentDate { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
// The ID property becomes the primary key column of the database table that corresponds to this class
// The Enrollments property is a navigation property. Navigation properties hold other entities that are related to this entity
// the Enrollments property of a Student entity holds all of the Enrollment entities that are related to that Student