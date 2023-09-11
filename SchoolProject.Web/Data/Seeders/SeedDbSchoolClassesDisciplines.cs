using System.Drawing;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Disciplines;

using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Data.Seeders;

/// <summary>
///     This class is used to seed the database with some initial data.
/// </summary>
public static class SeedDbSchoolClassesDisciplines
{
    private static List<Discipline> _listOfDisciplinesToAdd = new();
    private static List<Course> _listOfCoursesToAdd = new();


    // Your existing code for fetching existing school classes and courses
    public static async Task AddingData(User user,
        // DataContextMsSql dataContextInUse, 
        DataContextMySql dataContextInUse
    )
    {
        Console.WriteLine("debug zone...");


        // ------------------------------------------------------------------ //
        var existingCourses =
            await dataContextInUse.Disciplines.ToListAsync();

        // ------------------------------------------------------------------ //
        var existingSchoolClasses =
            await dataContextInUse.Courses
                // To Ensure Disciplines are loaded for each Discipline
                .Include(c => c.Disciplines)
                .ToListAsync();

        // ------------------------------------------------------------------ //
        _listOfDisciplinesToAdd = existingCourses.ToList();
        _listOfCoursesToAdd = existingSchoolClasses.ToList();


        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");
        if (await dataContextInUse.CoursesDisciplines.AnyAsync()) return;


        // Loop through each school class
        foreach (var schoolClass in _listOfCoursesToAdd)
        {
            // Check if Disciplines is null or empty before iterating
            if (schoolClass.Disciplines != null && schoolClass.Disciplines.Any())
                // Loop through each course associated with the school class
                foreach (var schoolClassCourse in
                         schoolClass.Disciplines.Select(
                             course => new CourseDisciplines
                             {
                                 CourseId = schoolClass.Id,
                                 Course = schoolClass,
                                 DisciplineId = course.Id,
                                 Discipline = course,
                                 CreatedBy = user
                             }))
                    // Add the association to the Discipline's CourseDisciplines collection
                    dataContextInUse.CoursesDisciplines.Add(schoolClassCourse);

            // ------------------------------------------------------------------ //
            Console.WriteLine("debug zone...", Color.Red);

            // Save the changes to the database
            await dataContextInUse.SaveChangesAsync();
        }


        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...", Color.Red);
    }
}