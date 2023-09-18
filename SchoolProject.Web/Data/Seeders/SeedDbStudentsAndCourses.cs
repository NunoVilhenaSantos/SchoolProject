using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Data.Seeders;

/// <summary>
/// </summary>
public class SeedDbStudentsAndCourses
{
    // private static DataContextMsSql _dataContextMsSql;
    // private static DataContextMsSql _dataContextInUse;
    private static readonly DataContextMySql _dataContextInUse;


    // public static void Initialize(DataContextMsSql dataContextMsSql)
    // {
    //     _dataContextMsSql = dataContextMsSql;
    // }

    public static async Task AddingData(
        User user, DataContextMySql dataContextInUse)
    {
        var _dataContextInUse = dataContextInUse;

        // ------------------------------------------------------------------ //
        // Get all school classes from the database
        var courses =
            await dataContextInUse.Courses.ToListAsync();
        courses.ToList();


        // ------------------------------------------------------------------ //
        // Get all students from the database
        var students =
            await dataContextInUse.Students.ToListAsync();
        students.ToList();


        // ------------------------------------------------------------------ //
        // Get all students from the database
        var courseStudentsList =
            await dataContextInUse.CoursesStudents.ToListAsync();
        courseStudentsList.ToList();


        // ------------------------------------------------------------------ //
        // Verifica se já há dados para popular a lista de school-classes dos estudantes
        if (!courses.Any() || courseStudentsList.Any()) return;


        // ------------------------------------------------------------------ //
        // Create a random number generator
        var random = new Random();


        // Collect new associations in memory
        var newAssociations = new HashSet<( int CourseId, int StudentId)>();

        foreach (var student in students)
        {
            var numberOfCourses = random.Next(1, 4);

            for (var i = 0; i < numberOfCourses; i++)
            {
                var randomCourse =
                    courses[random.Next(courses.Count)];

                // Check if the association already exists in the database
                newAssociations.Add((randomCourse.Id, student.Id));
            }
        }

        foreach (var (courseId, studentId) in newAssociations)
        {
            var course = courses.FirstOrDefault(c => c.Id == courseId);
            var student = students.FirstOrDefault(s => s.Id == studentId);

            if (course == null || student == null) continue;

            var courseStudents = new CourseStudents
            {
                CourseId = course.Id,
                Course = course,
                StudentId = student.Id,
                Student = student,
                CreatedBy = user,
                CreatedById = user.Id,
            };

            _dataContextInUse.CoursesStudents.Add(courseStudents);
        }

        // Save the changes to the database
        await dataContextInUse.SaveChangesAsync();
    }
}