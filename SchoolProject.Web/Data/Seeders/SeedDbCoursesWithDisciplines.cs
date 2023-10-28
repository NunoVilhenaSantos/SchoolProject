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
    /// <param name="appUser"></param>
    /// <param name="dataContextInUse"></param>
    public static async Task AddingData(AppUser appUser,
        DataContextMySql dataContextInUse // DataContextMsSql dataContextInUse,
    )
    {
        Console.WriteLine("debug zone...");


        // ------------------------------------------------------------------ //
        var existingDisciplines =
            await dataContextInUse.Disciplines.ToListAsync();

        // ------------------------------------------------------------------ //
        // To Ensure Disciplines are loaded for each Course
        var existingCourses = await dataContextInUse.Courses
            .Include(c => c.CourseDisciplines)
            .ToListAsync();


        // ------------------------------------------------------------------ //
        _listOfDisciplinesToAdd = existingDisciplines.ToList();
        _listOfCoursesToAdd = existingCourses.ToList();


        // ---------------------------------------------------------------- //
        Console.WriteLine("debug zone...");
        if (await dataContextInUse.CourseDisciplines.AnyAsync()) return;


        // Loop through Courses
        foreach (var course in _listOfCoursesToAdd)
        {
            // Check if Disciplines is null or empty before iterating
            if (course.CourseDisciplines != null &&
                course.CourseDisciplines.Any())
                // Loop through each course associated with the school class
                foreach (var disciplines in
                         course.CourseDisciplines.Select(
                             discipline => new CourseDiscipline
                             {
                                 CourseId = course.Id,
                                 Course = course,
                                 DisciplineId = discipline.Discipline.Id,
                                 Discipline = discipline.Discipline,
                                 CreatedBy = appUser,
                                 CreatedById = appUser.Id,
                             }))
                    // Add the association to Courses and Discipline's CourseDisciplines collection
                    dataContextInUse.CourseDisciplines.Add(disciplines);

            // ------------------------------------------------------------ //
            Console.WriteLine("debug zone...", Color.Red);

            // Save the changes to the database
            await dataContextInUse.SaveChangesAsync();
        }

        // ---------------------------------------------------------------- //
        Console.WriteLine("debug zone...", Color.Red);
    }
}