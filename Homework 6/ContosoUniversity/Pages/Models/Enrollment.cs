using System.ComponentModel.DataAnnotations;

namespace ContosoUniversity.Models
{
    public enum Grade
    {
        A, B, C, D, F
    }

    public class Enrollment
    {
        public int EnrollmentID { get; set; }
        public int CourseID { get; set; }
        public int StudentID { get; set; }
        [DisplayFormat(NullDisplayText = "No grade")]
        public Grade? Grade { get; set; }

        public Course Course { get; set; }
        public Student Student { get; set; }
    }
}
// The EnrollmentID property is the primary key
// this entity uses the classnameID pattern instead of ID by itself

// The Grade property is an enum. The question mark after the Grade type declaration indicates that the Grade property is nullable
// he StudentID property is a foreign key, and the corresponding navigation property is Student
// The CourseID property is a foreign key, and the corresponding navigation property is Course
