using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Seeders;

public class SeedDbTeachersWithCourses
{
    private static DataContextMsSql _dataContextMsSql;

    private static List<Course> _listOfCoursesFromDb;
    private static List<Teacher> _listOfTeachersFromDb;


    public static void Initialize(DataContextMsSql dataContextMsSql)
    {
        _dataContextMsSql = dataContextMsSql;
    }


    public static async Task AddingData(User user)
    {
        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");


        // ------------------------------------------------------------------ //
        var existingCourses =
            await _dataContextMsSql.Courses.ToListAsync();
        _listOfCoursesFromDb = existingCourses.ToList();


        // ------------------------------------------------------------------ //
        var existingTeachers =
            await _dataContextMsSql.Teachers.ToListAsync();
        _listOfTeachersFromDb = existingTeachers.ToList();


        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");


        // ------------------------------------------------------------------ //
        await using (var context = _dataContextMsSql)
        {
            var random = new Random();

            foreach (var teacherCourse in from course in _listOfCoursesFromDb
                     let teacher = _listOfTeachersFromDb[
                         random.Next(_listOfTeachersFromDb.Count)]
                     select new TeacherCourse
                     {
                         Teacher = teacher,
                         Course = course,
                         CreatedBy = user,
                     })
            {
                // Add the TeacherCourse association to the context
                context.TeacherCourses.Add(teacherCourse);
            }


            // -------------------------------------------------------------- //
            Console.WriteLine("debug zone...");

            // Save the changes to the database
            await context.SaveChangesAsync();
        }


        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");
    }
}