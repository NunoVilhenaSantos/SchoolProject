using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.School;
using SchoolProject.Web.Data.Entities.Students;
using Serilog;

namespace SchoolProject.Web.Data.Entities.Enrollments;

public static class Enrollments
{
    public static List<Enrollment> ListEnrollments = new();


    /// <summary>
    ///     used to update the dictionaries from the list of their corresponding classes
    /// </summary>
    public static void UpdateDictionaries()
    {
        // update the students dictionary
        foreach (var student in Students.Students.StudentsList)
            Students.Students.StudentsDictionary[student.Id] = student;

        // update the courses dictionary
        foreach (var course in Courses.Courses.CoursesList)
            Courses.Courses.CoursesDictionary[course.Id] = course;
    }

    /// <summary>
    ///     enroll students in courses
    /// </summary>
    public static void AddEnrollmentsToSchoolDatabase()
    {
        // update the students dictionary
        foreach (var e in ListEnrollments)
            SchoolDatabase.EnrollStudentInCourse(e.Student, e.Course);
    }

    /// <summary>
    ///     Enroll a student in a course
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="courseId"></param>
    /// <param name="grade"></param>
    public static void EnrollStudent(
        Student student, Course course, decimal? grade = null)
    {
        // Check if the student ID is valid
        if (!Students.Students.StudentsDictionary.ContainsKey(student.Id))
        {
            Log.Error("Invalid student ID: " +
                      "{StudentId}", student.Id);
            return;
        }

        // Check if the course ID is valid
        if (!Courses.Courses.CoursesDictionary.ContainsKey(course.Id))
        {
            Log.Error("Invalid course ID: " +
                      "{CourseId}", course.Id);
            return;
        }

        // Check if the student is already enrolled in the course
        if (ListEnrollments
            .Any(e =>
                e.Id == student.Id &&
                e.Id == course.Id))
        {
            Log.Error("Student {StudentId} " +
                      "is already enrolled in course {CourseId}",
                student.Id, course.Id);
            return;
        }

        // Create a new enrollment and add it to the list
        var newEnrollment = new Enrollment
        {
            Grade = grade,
            Student = student,
            Course = course,
            CreatedBy = _,
        };
        ListEnrollments.Add(newEnrollment);

        // Update the school database
        SchoolDatabase.EnrollStudentInCourse(student, course);
    }


    /// <summary>
    ///     unenroll a student from a course
    /// </summary>
    /// <param name="idStudent"></param>
    /// <param name="idCourse"></param>
    public static void UnenrollStudent(Student student, Course course)
    {
        var enrollment = ListEnrollments.FirstOrDefault(e =>
            e.Student == student && e.Course == course);

        if (enrollment == null)
        {
            Log.Warning(
                "Attempted to unenroll student {StudentId} " +
                "from course {CourseId}," +
                " but no such enrollment exists",
                student.Id, course.Id);
            return;
        }

        ListEnrollments.Remove(enrollment);
        Log.Information(
            "Student {StudentId} " +
            "has been unenrolled from course {CourseId}",
            student.Id, course.Id);
    }

    /// <summary>
    ///     remove an enrollment from the list
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="courseId"></param>
    public static void RemoveEnrollment(Student student, Course course)
    {
        var enrollment = ListEnrollments.FirstOrDefault(
            e => e.Student == student && e.Course == course);

        if (enrollment == null)
        {
            Log.Warning(
                "Attempted to unenroll student {StudentId} " +
                "from course {CourseId}," +
                " but no such enrollment exists",
                student.Id, course.Id);
            return;
        }

        ListEnrollments.Remove(enrollment);
    }


    /// <summary>
    ///     list all enrollments according to the parameters
    /// </summary>
    /// <param name="courseId"></param>
    /// <param name="studentId"></param>
    /// <returns>
    ///     list all enrollments according to the parameters
    ///     studentID ou courseID
    /// </returns>
    public static List<Enrollment> ConsultEnrollment(
        int courseId = -1, int studentId = -1)
    {
        var enrollments = ListEnrollments;

        if (courseId != -1)
            if (Courses.Courses.CoursesDictionary
                .TryGetValue(courseId, out var course))
                enrollments = enrollments
                    .Where(e =>
                        e.Course.Id == course.Id)
                    .ToList();

        if (studentId == -1) return enrollments;
        {
            if (Students.Students.StudentsDictionary
                .TryGetValue(studentId, out var student))
                enrollments = enrollments
                    .Where(e =>
                        e.Student.Id == student.Id)?
                    .ToList();
        }

        return enrollments;
    }

    /// <summary>
    ///     Searching an enrollment
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="courseId"></param>
    /// <param name="grade"></param>
    /// <returns>
    ///     Returns a list of enrollments
    ///     by student or course or grade
    /// </returns>
    public static List<Enrollment> ConsultEnrollment(
        Student student, Course course, decimal? grade)
    {
        var enrollments = ListEnrollments;

        if (student.Id != -1)
            enrollments = enrollments
                .Where(a => a.Student.Id == student.Id)
                .ToList();

        if (course.Id != -1)
            enrollments = enrollments
                .Where(a => a.Course.Id == course.Id)
                .ToList();

        if (grade.HasValue)
            enrollments = enrollments
                .Where(a => a.Grade == grade.Value)
                .ToList();

        return enrollments;
    }


    /// <summary>
    ///     Searching an enrollment
    /// </summary>
    /// <param name="id"></param>
    /// <param name="grade"></param>
    /// <param name="studentId"></param>
    /// <param name="courseId"></param>
    /// <returns>
    ///     Returns a list of enrollments
    ///     by student or course or grade
    /// </returns>
    public static List<Enrollment> ConsultEnrollment(
        int id, decimal? grade,
        Student student, Course course)
    {
        var enrollments = ListEnrollments;

        if (id >= 0)
            enrollments =
                enrollments.Where(e => e.Id == id).ToList();

        if (grade.HasValue)
            enrollments =
                enrollments.Where(e => e.Grade == grade).ToList();

        if (student.Id >= 0)
            enrollments =
                enrollments.Where(e => e.Student.Id == student.Id).ToList();

        if (course.Id >= 0)
            enrollments =
                enrollments.Where(e => e.Course.Id == course.Id).ToList();

        return enrollments;
    }
}