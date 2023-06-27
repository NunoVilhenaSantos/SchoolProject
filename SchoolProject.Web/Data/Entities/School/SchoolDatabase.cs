using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using Serilog;

namespace SchoolProject.Web.Data.Entities.School;

public class SchoolDatabase
{
    #region UpdatingDictionaries

    // update the dictionaries from the list of their corresponding classes

    /// <summary>
    ///     used to update the dictionaries from
    ///     the list of their corresponding classes
    /// </summary>
    public static void UpdateDictionaries()
    {
        // update the Courses dictionary
        foreach (var course in Courses.Courses.CoursesList)
            Courses.Courses.CoursesDictionary[course.Id] = course;

        // update the SchoolClasses dictionary
        foreach (var schoolClass in
                 SchoolClasses.SchoolClasses.SchoolClassesList)
            SchoolClasses.SchoolClasses
                .SchoolClassesDictionary[schoolClass.Id] = schoolClass;

        // update the Students dictionary
        foreach (var student in Students.Students.StudentsList)
            Students.Students.StudentsDictionary[student.Id] = student;

        // update the Teachers dictionary
        foreach (var teacher in Teachers.Teachers.TeachersList)
            Teachers.Teachers.TeachersDictionary[teacher.Id] = teacher;
    }

    #endregion

    #region Constructor

    //
    // Região com o construtor da diversas classes
    //


    // public SchoolDatabase()
    // {
    //     SchoolClassCourses = new Dictionary<int, HashSet<int>>();
    //     StudentCourses = new Dictionary<int, HashSet<int>>();
    //     TeacherCourses = new Dictionary<int, HashSet<int>>();
    // }

    #endregion


    #region Properties

    #region DictionaryClasses

    //
    // Região com os dicionários das classes primitivas
    //
    // Dictionaries to store the objects
    // public static readonly Dictionary<int, Course> Courses = new();
    // public static readonly Dictionary<int, SchoolClass> SchoolClasses = new();
    // public static readonly Dictionary<int, Student> Students = new();
    // public static readonly Dictionary<int, Teacher> Teachers = new();

    #endregion


    #region DictionaryRelations

    // modificado por mim
    // Dictionaries to store the relationships
    public static readonly Dictionary<int, HashSet<int>> CourseClasses = new();
    public static readonly Dictionary<int, HashSet<int>> CourseStudents = new();
    public static readonly Dictionary<int, HashSet<int>> StudentClass = new();
    public static readonly Dictionary<int, HashSet<int>> CourseTeacher = new();

    #endregion

    #endregion


    #region AddingValuesToPrimaryClasses

    //
    // região para adicionar os membros das classes primarias
    //


    /// <summary>
    ///     add a course to the Courses dictionary
    /// </summary>
    /// <param name="course"></param>
    public static void AddCourse(Course course)
    {
        Courses.Courses.CoursesDictionary.Add(course.Id, course);
        // Courses[course.Id] = course;
    }


    /// <summary>
    ///     add a school class to the SchoolClasses dictionary
    /// </summary>
    /// <param name="schoolClass"></param>
    public static void AddSchoolClass(SchoolClass schoolClass)
    {
        SchoolClasses.SchoolClasses.SchoolClassesDictionary
            .Add(schoolClass.Id, schoolClass);
        // SchoolClasses.SchoolClasses
        //     .SchoolClassesDictionary[schoolClass.IdSchoolClass] = schoolClass;
    }


    /// <summary>
    ///     add a student to the Students dictionary
    /// </summary>
    /// <param name="student"></param>
    public static void AddStudent(Student? student)
    {
        if (student != null)
            Students.Students.StudentsDictionary.Add(
                student.Id, student);
        // Students[student.Id] = student;
    }


    /// <summary>
    ///     add a teacher to the Teachers dictionary
    /// </summary>
    /// <param name="teacher"></param>
    public static void AddTeacher(Teacher teacher)
    {
        Teachers.Teachers.TeachersDictionary.Add(teacher.Id, teacher);
        // Teachers[teacher.TeacherId] = teacher;
    }

    #endregion


    #region AddingRelationsBetweenPrimaryClasses

    #region EnrollStudentInClass

    // Method to add a relationship between a student and a school class
    /// <summary>
    ///     enroll a student in a school class
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="schoolClassId"></param>
    public static void EnrollStudentInClass(int studentId, int schoolClassId)
    {
        if (!StudentClass.ContainsKey(studentId))
        {
            StudentClass.Add(studentId, new HashSet<int>());
            StudentClass[studentId] = new HashSet<int> {studentId};
        }

        StudentClass[studentId].Add(schoolClassId);
    }

    /// <summary>
    ///     enroll a list of students in a school class
    /// </summary>
    /// <param name="listStudents"></param>
    /// <param name="schoolClassId"></param>
    public static void EnrollStudentsInClass(
        List<Student> listStudents, int schoolClassId)
    {
        foreach (var student in listStudents)
        {
            if (!StudentClass.ContainsKey(student.Id))
            {
                StudentClass.Add(student.Id, new HashSet<int>());
                StudentClass[student.Id] = new HashSet<int> {student.Id};
            }

            StudentClass[student.Id].Add(schoolClassId);
        }
    }

    #endregion


    #region EnrollStudentInCourse

    /// <summary>
    ///     enroll students in a courses
    /// </summary>
    /// <param name="listOfCourses"></param>
    /// <param name="listOfStudents"></param>
    public static void EnrollStudentsInCourses(
        List<Course> listOfCourses, List<Student> listOfStudents)
    {
        foreach (var course in listOfCourses)
        foreach (var student in listOfStudents)
        {
            if (!Students.Students.StudentsDictionary
                    .ContainsKey(student.Id))
            {
                Log.Error("Invalid student ID: " +
                          "{StudentId}", student.Id);
                continue;
            }

            if (!Courses.Courses.CoursesDictionary
                    .ContainsKey(course.Id))
            {
                Log.Error("Invalid course ID: " +
                          "{CourseId}", course.Id);
                continue;
            }

            if (CourseStudents
                    .TryGetValue(student.Id,
                        out var currentCourse) &&
                currentCourse.Contains(course.Id))
            {
                Log.Warning("Student {StudentId} " +
                            "is already enrolled in course {CourseId}",
                    student.Id,
                    course.Id);
                continue;
            }

            Enrollments.Enrollments.EnrollStudent(student, course);

            if (!CourseStudents.ContainsKey(student.Id))
                CourseStudents.Add(student.Id, new HashSet<int>());

            CourseStudents[student.Id].Add(course.Id);
        }
    }

    /// <summary>
    ///     enroll a student in a course
    /// </summary>
    /// <param name="studentId"></param>
    /// <param name="courseId"></param>
    public static void EnrollStudentInCourse(Student student, Course course)
    {
        if (!Students.Students.StudentsDictionary
                .ContainsKey(student.Id))
        {
            Log.Error(
                "Invalid student ID: {StudentId}", student.Id);
            return;
        }

        if (!Courses.Courses.CoursesDictionary
                .ContainsKey(course.Id))
        {
            Log.Error(
                "Invalid course ID: {CourseId}", course.Id);
            return;
        }

        if (CourseStudents
                .TryGetValue(student.Id, out var courses) &&
            courses.Contains(course.Id))
        {
            Log.Warning(
                "Student {StudentId} " +
                "is already enrolled in course {CourseId}",
                student.Id, course.Id);
            return;
        }

        Enrollments.Enrollments.EnrollStudent(student, course);

        if (!CourseStudents.ContainsKey(student.Id))
            CourseStudents.Add(student.Id, new HashSet<int>());

        CourseStudents[student.Id].Add(course.Id);
    }


    /// <summary>
    ///     enroll a list of students in a course
    /// </summary>
    /// <param name="students"></param>
    /// <param name="courseId"></param>
    public static void EnrollStudentsInCourse(
        List<Student> students, Course course)
    {
        foreach (var student in students)
        {
            if (!Students.Students.StudentsDictionary
                    .ContainsKey(student.Id))
            {
                Log.Error("Invalid student ID: " +
                          "{StudentId}", student.Id);
                continue;
            }

            if (!Courses.Courses.CoursesDictionary
                    .ContainsKey(course.Id))
            {
                Log.Error("Invalid course ID: " +
                          "{CourseId}", course.Id);
                continue;
            }

            if (CourseStudents.TryGetValue(student.Id,
                    out var courses) && courses.Contains(course.Id))
            {
                Log.Warning("Student {StudentId} " +
                            "is already enrolled in course {CourseId}",
                    student.Id, course.Id);
                continue;
            }

            Enrollments.Enrollments.EnrollStudent(student, course);

            if (!CourseStudents.ContainsKey(student.Id))
                CourseStudents.Add(student.Id, new HashSet<int>());

            CourseStudents[student.Id].Add(course.Id);
        }
    }


    /// <summary>
    ///     unenroll a student from a list of courses
    /// </summary>
    /// <param name="coursesToRemove"></param>
    /// <param name="Id"></param>
    public static void UnenrollStudentFromCourses(
        List<Course> coursesToRemove, Student student)
    {
        foreach (var course in coursesToRemove)
        {
            if (!Students.Students.StudentsDictionary
                    .ContainsKey(student.Id))
            {
                Log.Error(
                    "Invalid student ID: {StudentId}",
                    student.Id);
                continue;
            }

            if (!Courses.Courses.CoursesDictionary
                    .ContainsKey(course.Id))
            {
                Log.Error(
                    "Invalid course ID: {CourseId}",
                    course.Id);
                continue;
            }

            if (!CourseStudents.TryGetValue(student.Id, out var courses) ||
                !courses.Contains(course.Id))
            {
                Log.Warning(
                    "Student {StudentId} " +
                    "is not enrolled in course {CourseId}",
                    student.Id, course.Id);
                continue;
            }

            Enrollments.Enrollments.UnenrollStudent(student, course);

            CourseStudents[student.Id].Remove(course.Id);

            if (CourseStudents[student.Id].Count == 0)
                CourseStudents.Remove(student.Id);
        }
    }

    /// <summary>
    ///     unenroll a list of student from a course
    /// </summary>
    /// <param name="students"></param>
    /// <param name="courseId"></param>
    public static void UnenrollStudentsFromCourse(
        List<Student> students, Course course)
    {
        foreach (var student in students)
        {
            if (!Students.Students.StudentsDictionary
                    .ContainsKey(student.Id))
            {
                Log.Error(
                    "Invalid student ID: {StudentId}",
                    student.Id);
                continue;
            }

            if (!Courses.Courses.CoursesDictionary
                    .ContainsKey(course.Id))
            {
                Log.Error(
                    "Invalid course ID: {CourseId}",
                    course.Id);
                continue;
            }

            if (!CourseStudents.TryGetValue(course.Id,
                    out var enrolledStudents) ||
                !enrolledStudents.Contains(student.Id))
            {
                Log.Warning(
                    "Student {StudentId} is not " +
                    "enrolled in course {CourseId}",
                    student.Id, course.Id);
                continue;
            }

            Enrollments.Enrollments.UnenrollStudent(
                student, course);

            CourseStudents[course.Id].Remove(student.Id);

            if (CourseStudents[course.Id].Count == 0)
                CourseStudents.Remove(course.Id);
        }
    }


    /// <summary>
    ///     enroll a student in a list of courses
    /// </summary>
    /// <param name="listOfCourses"></param>
    /// <param name="Id"></param>
    public static void EnrollStudentInCourses(
        List<Course> listOfCourses, Student student)
    {
        foreach (var course in listOfCourses)
        {
            if (!Students.Students.StudentsDictionary
                    .ContainsKey(student.Id))
            {
                Log.Error("Invalid student ID: " +
                          "{StudentId}", student.Id);
                continue;
            }

            if (!Courses.Courses.CoursesDictionary
                    .ContainsKey(course.Id))
            {
                Log.Error("Invalid course ID: " +
                          "{CourseId}", course.Id);
                continue;
            }

            if (
                CourseStudents.TryGetValue(student.Id,
                    out var courses) &&
                courses.Contains(course.Id))
            {
                Log.Warning("Student {StudentId} " +
                            "is already enrolled in course {CourseId}",
                    student.Id, course.Id);
                continue;
            }

            Enrollments.Enrollments.EnrollStudent(student, course);

            if (!CourseStudents.ContainsKey(student.Id))
                CourseStudents.Add(student.Id, new HashSet<int>());

            CourseStudents[student.Id].Add(course.Id);
        }
    }


    /// <summary>
    ///     enroll a student in a list of courses, using a HashSet
    /// </summary>
    /// <param name="listOfCourses"></param>
    /// <param name="Id"></param>
    public static void EnrollStudentInCourses(
        HashSet<int> listOfCourses, Student student)
    {
        foreach (var course in listOfCourses)
        {
            if (!Students.Students.StudentsDictionary
                    .ContainsKey(student.Id))
            {
                Log.Error("Invalid student ID: " +
                          "{StudentId}", student.Id);
                continue;
            }

            if (!Courses.Courses.CoursesDictionary
                    .ContainsKey(course))
            {
                Log.Error("Invalid course ID: " +
                          "{CourseId}", course);
                continue;
            }

            if (
                CourseStudents.TryGetValue(student.Id,
                    out var courses) &&
                courses.Contains(course))
            {
                Log.Warning("Student {StudentId} " +
                            "is already enrolled in course {CourseId}",
                    student.Id, course);
                continue;
            }

            _ = Courses.Courses.CoursesDictionary
                .TryGetValue(course, out var courseCourse);
            if (courseCourse != null)
                Enrollments.Enrollments.EnrollStudent(student, courseCourse);

            if (!CourseStudents.ContainsKey(student.Id))
                CourseStudents.Add(student.Id, new HashSet<int>());

            CourseStudents[student.Id].Add(course);
        }
    }

    #endregion


    /// <summary>
    ///     assign a course to a class
    /// </summary>
    /// <param name="courseId"></param>
    /// <param name="schoolClassId"></param>
    public static void AssignCourseToClass(int courseId, int schoolClassId)
    {
        if (Courses.Courses.CoursesDictionary
                .TryGetValue(courseId, out var course) &&
            SchoolClasses.SchoolClasses.SchoolClassesDictionary
                .TryGetValue(schoolClassId, out var schoolClass))
        {
            if (!CourseClasses
                    .TryGetValue(courseId, out var classes))
            {
                classes = new HashSet<int>();
                CourseClasses.Add(courseId, classes);
            }

            classes.Add(schoolClassId);

            Log.Information(
                "Assigned course {CourseId} to class {ClassId}",
                courseId, schoolClassId);
        }
        else
        {
            Log.Warning(
                "Failed to assign course " +
                "{CourseId} to class {ClassId}." +
                " Course or school class does not exist",
                courseId, schoolClassId);
        }
    }

    /// <summary>
    ///     assign a list of courses to a class
    /// </summary>
    /// <param name="listOfCourses"></param>
    /// <param name="schoolClassId"></param>
    public static void AssignCoursesToClass(
        List<Course> listOfCourses, int schoolClassId)
    {
        if (!SchoolClasses.SchoolClasses.SchoolClassesDictionary
                .TryGetValue(schoolClassId, out var schoolClass))
        {
            Log.Warning(
                "Failed to assign courses to class {ClassId}." +
                " School class does not exist",
                schoolClassId);
            return;
        }

        foreach (var course in listOfCourses)
            if (Courses.Courses.CoursesDictionary
                .TryGetValue(course.Id, out var c))
            {
                if (!CourseClasses.TryGetValue(
                        course.Id,
                        out var classes))
                {
                    classes = new HashSet<int>();
                    CourseClasses.Add(course.Id, classes);
                }

                classes.Add(schoolClassId);

                Log.Information(
                    "Assigned course {CourseId} " +
                    "to class {ClassId}",
                    course.Id, schoolClassId);
            }
            else
            {
                Log.Warning(
                    "Failed to assign course {CourseId} " +
                    "to class {ClassId}. Course does not exist",
                    course.Id, schoolClassId);
            }
    }

    /// <summary>
    ///     assign a list of courses to a class, using a HashSet
    /// </summary>
    /// <param name="listOfCourses"></param>
    /// <param name="schoolClassId"></param>
    public static void AssignCoursesToClass(
        HashSet<int> listOfCourses, int schoolClassId)
    {
        if (!SchoolClasses.SchoolClasses.SchoolClassesDictionary
                .TryGetValue(schoolClassId, out var schoolClass))
        {
            Log.Warning(
                "Failed to assign courses to class {ClassId}." +
                " School class does not exist",
                schoolClassId);
            return;
        }

        foreach (var course in listOfCourses)
            if (Courses.Courses.CoursesDictionary
                .TryGetValue(course, out var c))
            {
                if (!CourseClasses.TryGetValue(course,
                        out var classes))
                {
                    classes = new HashSet<int>();
                    CourseClasses.Add(course, classes);
                }

                classes.Add(schoolClassId);

                Log.Information(
                    "Assigned course {CourseId} " +
                    "to class {ClassId}",
                    course, schoolClassId);
            }
            else
            {
                Log.Warning(
                    "Failed to assign course {CourseId} " +
                    "to class {ClassId}. Course does not exist",
                    course, schoolClassId);
            }
    }


    #region AssignTeachersToCourses

    /// <summary>
    ///     assign a teacher to a course
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="courseId"></param>
    public static void AssignTeacherToCourse(int teacherId, int courseId)
    {
        if (!CourseTeacher.ContainsKey(teacherId))
        {
            CourseTeacher.Add(teacherId, new HashSet<int>());
            CourseTeacher[teacherId] = new HashSet<int>();
        }

        CourseTeacher[teacherId].Add(courseId);
    }

    /// <summary>
    ///     assign a teacher to a list of courses
    /// </summary>
    /// <param name="teacherId"></param>
    /// <param name="courses"></param>
    public static void AssignTeacherToCourses(
        int teacherId, List<Course> courses)
    {
        foreach (var course in courses)
        {
            if (!CourseTeacher.ContainsKey(teacherId))
            {
                CourseTeacher.Add(teacherId, new HashSet<int>());
                CourseTeacher[teacherId] = new HashSet<int>();
            }

            CourseTeacher[teacherId].Add(course.Id);
        }
    }


    /// <summary>
    ///     assign a teacher to a list of courses, using a HashSet
    /// </summary>
    /// <param name="hashSetCourses"></param>
    /// <param name="teacherId"></param>
    public static void AssignTeacherToCourses(
        HashSet<int> hashSetCourses, int teacherId)
    {
        foreach (var course in hashSetCourses)
        {
            if (!CourseTeacher.ContainsKey(teacherId))
            {
                CourseTeacher.Add(teacherId, new HashSet<int>());
                CourseTeacher[teacherId] = new HashSet<int>();
            }

            CourseTeacher[teacherId].Add(course);
        }
    }

    #endregion

    #endregion


    #region IEnumerar

    // enumerating zone

    /// <summary>
    ///     get all courses for a school class
    /// </summary>
    /// <param name="schoolClassId"></param>
    /// <returns> get all courses for a school class </returns>
    public static List<Course> GetCoursesForSchoolClass(
        int schoolClassId)
    {
        if (!CourseClasses.ContainsKey(schoolClassId))
            return new List<Course>();

        return CourseClasses[schoolClassId]
            .Select(x => Courses.Courses.CoursesDictionary[x])
            .ToList();
    }


    // Get all courses a student is enrolled in
    /// <summary>
    ///     Get all courses a student is enrolled in
    /// </summary>
    /// <param name="studentId"></param>
    /// <returns>
    ///     Get all courses a student is enrolled in,
    ///     returning a list of courses
    /// </returns>
    public static List<Course> GetCoursesForStudent(int studentId)
    {
        if (CourseStudents.TryGetValue(studentId, out var studentCourses))
        {
            var courses = new List<Course>();
            foreach (var courseId in studentCourses)
                if (Courses.Courses.CoursesDictionary
                    .TryGetValue(courseId, out var course))
                    courses.Add(course);
                else
                    Log.Information(
                        "Invalid course id {courseId}",
                        courseId);

            return courses;
        }

        Log.Information(
            "Invalid student id {studentId}",
            studentId);

        return new List<Course>();
    }


    public static List<Course> GetCoursesForTeacher(int teacherId)
    {
        if (!CourseTeacher.ContainsKey(teacherId)) return new List<Course>();

        return CourseTeacher[teacherId]
            .Select(x => Courses.Courses.CoursesDictionary[x])
            .ToList();
    }


    // Get all students enrolled in a course
    public static List<Student> GetStudentsInCourse(int courseId)
    {
        if (!Courses.Courses.CoursesDictionary
                .ContainsKey(courseId))
        {
            Log.Information(
                "Invalid course id: {CourseId}", courseId);
            return new List<Student>();
        }

        if (CourseStudents.TryGetValue(courseId, out var student))
            return student
                .Select(x => Students.Students.StudentsDictionary[x])
                .ToList();

        Log.Information(
            "No students enrolled in course: {CourseId}",
            courseId);

        return new List<Student>();
    }


    public static Dictionary<int, HashSet<Student>>
        GetEnrolledStudentsByCourseForClass(int schoolClassId)
    {
        var enrolledStudentsByCourse = new Dictionary<int, HashSet<Student>>();
        var errorMessage =
            $"Error retrieving students enrolled in " +
            $"courses for school class with ID {schoolClassId}.";

        if (!CourseClasses.ContainsKey(schoolClassId))
        {
            var errorDetails =
                $"Course classes not found for " +
                $"school class with ID {schoolClassId}.";
            Log.Error($"{errorMessage} {errorDetails}");
            return enrolledStudentsByCourse;
        }

        var courseIds = CourseClasses[schoolClassId];

        foreach (var courseId in courseIds)
        {
            if (!CourseStudents.ContainsKey(courseId))
            {
                var errorDetails =
                    $"Course students not found for course with ID {courseId}.";
                Log.Warning($"{errorMessage} {errorDetails}");
                continue;
            }

            var studentIds = CourseStudents[courseId];
            var students = new HashSet<Student>();

            foreach (var studentId in studentIds)
            {
                if (!StudentClass.ContainsKey(studentId))
                {
                    var errorDetails =
                        $"Student classes not found for " +
                        $"student with ID {studentId}.";
                    Log.Warning($"{errorMessage} {errorDetails}");
                    continue;
                }

                if (StudentClass[studentId].Contains(schoolClassId) &&
                    Students.Students.StudentsDictionary
                        .TryGetValue(studentId, out var student))
                    students.Add(student);
            }

            enrolledStudentsByCourse.Add(courseId, students);
        }

        var totalStudents =
            enrolledStudentsByCourse
                .Sum(c => c.Value.Count);
        Log.Information(
            $"Retrieved {totalStudents} students enrolled " +
            $"in courses for school class with ID {schoolClassId}.");
        return enrolledStudentsByCourse;
    }

    #endregion


    #region GetList

    public static int GetCoursesCountForSchoolClass(int schoolClassId)
    {
        var coursesCount =
            GetCoursesForSchoolClass(schoolClassId)?.Count ?? 0;

        if (coursesCount == 0)
            Log.Warning("No courses found for " +
                        "the specified school class");

        return coursesCount;
    }

    public static int GetWorkHourLoadForSchoolClass(int schoolClassId)
    {
        var listCoursesForSchoolClass =
            GetCoursesForSchoolClass(schoolClassId);
        var workHourLoad =
            listCoursesForSchoolClass?.Sum(c => c.WorkLoad) ?? 0;

        return workHourLoad;
    }

    public static int GetStudentsCountForSchoolClass(int schoolClassId)
    {
        var listCoursesForSchoolClass =
            GetCoursesForSchoolClass(schoolClassId);
        var studentsCount =
            listCoursesForSchoolClass?
                .Join(Enrollments.Enrollments.ListEnrollments,
                    c => c.Id,
                    e => e.Course.Id,
                    (c, e) => e).Count() ?? 0;

        return studentsCount;
    }

    public static (decimal, decimal, decimal) GetClassGradesForSchoolClass(
        int schoolClassId)
    {
        var enrollments = Enrollments.Enrollments.ListEnrollments;
        var courses = GetCoursesForSchoolClass(schoolClassId);

        var query =
            from enrollment in enrollments
            join course in courses on enrollment.Course.Id equals course.Id
            select new
            {
                enrollment,
                course
            };

        var classAverage =
            query.Average(ec => ec.enrollment.Grade) ?? 0;
        var highestGrade =
            query.Max(ec => ec.enrollment.Grade) ?? 0;
        var lowestGrade =
            query.Min(ec => ec.enrollment.Grade) ?? 0;

        return (classAverage, highestGrade, lowestGrade);
    }

    public static decimal GetStudentAveragesForSchoolClass(int schoolClassId)
    {
        var enrollments = Enrollments.Enrollments.ListEnrollments;
        var courses = GetCoursesForSchoolClass(schoolClassId);

        var query =
            from enrollment in enrollments
            join course in courses on enrollment.Course.Id equals course.Id
            select new
            {
                enrollment,
                course
            };

        var studentAverages =
            query
                .GroupBy(ec => ec.enrollment.Student.Id)
                .Select(g => new
                {
                    StudentId = g.Key,
                    AverageGrade = g.Average(ec => ec.enrollment.Grade)
                });

        var studentCountAverage =
            studentAverages
                .Average(sa => sa.AverageGrade) ?? 0;

        return studentCountAverage;
    }

    public static int GetStudentCountForSchoolClass(int schoolClassId)
    {
        var enrollments = Enrollments.Enrollments.ListEnrollments;
        var courses = GetCoursesForSchoolClass(schoolClassId);

        var query = from enrollment in enrollments
            join course in courses on enrollment.Course.Id equals course.Id
            select new
            {
                enrollment,
                course
            };

        var studentAverages =
            query
                .GroupBy(ec => ec.enrollment.Student.Id)
                .Select(g => new
                {
                    StudentId = g.Key,
                    AverageGrade = g.Average(ec => ec.enrollment.Grade)
                });

        var studentCount = studentAverages.Count();

        return studentCount;
    }

    public static void RemoveTeacherFromCourses(List<Course> coursesToRemove,
        int teacherId)
    {
        throw new NotImplementedException();
    }

    #endregion
}