using System.Drawing;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Data.Seeders;

/// <summary>
///     This class is used to seed the database with some initial data.
/// </summary>
public class SeedDbCoursesWithDisciplines
{
    private static List<Course> _listOfCoursesToAdd = new();
    private static List<Discipline> _listOfDisciplinesToAdd = new();


    /// <summary>
    /// code for fetching existing courses and disciplines
    /// </summary>
    /// <param name="user"></param>
    /// <param name="dataContextInUse"></param>
    public static async Task AddingData(User user,
        // DataContextMsSql dataContextInUse, 
        DataContextMySql dataContextInUse
    )
    {
        Console.WriteLine("debug zone...");


        // ------------------------------------------------------------------ //
        var existingDisciplines =
            await dataContextInUse.Disciplines.ToListAsync();

        // ------------------------------------------------------------------ //
        var existingCourses =
            await dataContextInUse.Courses
                // To Ensure Disciplines are loaded for each Discipline
                .Include(c => c.Disciplines)
                .ToListAsync();

        // ------------------------------------------------------------------ //
        _listOfDisciplinesToAdd = existingDisciplines.ToList();
        _listOfCoursesToAdd = existingCourses.ToList();


        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");
        if (await dataContextInUse.CoursesDisciplines.AnyAsync()) return;


        // Loop through each school class
        foreach (var course in _listOfCoursesToAdd)
        {
            // Check if Disciplines is null or empty before iterating
            if (course.Disciplines != null &&
                course.Disciplines.Any())
                // Loop through each course associated with the school class
                foreach (var disciplines in course.Disciplines.Select(
                             discipline => new CourseDisciplines
                             {
                                 CourseId = course.Id,
                                 Course = course,
                                 DisciplineId = discipline.Id,
                                 Discipline = discipline,
                                 CreatedBy = user,
                                 CreatedById = user.Id,
                             }))
                    // Add the association to Courses and Discipline's CourseDisciplines collection
                    dataContextInUse.CoursesDisciplines.Add(disciplines);

            // ------------------------------------------------------------------ //
            Console.WriteLine("debug zone...", Color.Red);

            // Save the changes to the database
            await dataContextInUse.SaveChangesAsync();
        }

        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...", Color.Red);
    }
}