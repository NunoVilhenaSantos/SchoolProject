using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.Entities.Users;
using System.Drawing;

namespace SchoolProject.Web.Data.Seeders;

/// <summary>
///     This class is used to seed the database with some initial data.
/// </summary>
public static class SeedDbSchoolClassesWithCourses
{
    private static List<Course> _listOfCoursesToAdd = new();
    private static List<SchoolClass> _listOfSchoolClassesToAdd = new();


    // Your existing code for fetching existing school classes and courses
    public static async Task AddingData(User user,
        // DataContextMsSql dataContextInUse, 
        DataContextMySql dataContextInUse
    )
    {
        Console.WriteLine("debug zone...");


        // ------------------------------------------------------------------ //
        var existingCourses =
            await dataContextInUse.Courses.ToListAsync();

        // ------------------------------------------------------------------ //
        var existingSchoolClasses =
            await dataContextInUse.SchoolClasses
                // To Ensure Courses are loaded for each SchoolClass
                .Include(sc => sc.Courses)
                .ToListAsync();

        // ------------------------------------------------------------------ //
        _listOfCoursesToAdd = existingCourses.ToList();
        _listOfSchoolClassesToAdd = existingSchoolClasses.ToList();


        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");
        if (await dataContextInUse.SchoolClassCourses.AnyAsync()) return;


        // Loop through each school class
        foreach (var schoolClass in _listOfSchoolClassesToAdd)
        {
            // Check if Courses is null or empty before iterating
            if (schoolClass.Courses != null && schoolClass.Courses.Any())
                // Loop through each course associated with the school class
                foreach (var schoolClassCourse in
                         schoolClass.Courses.Select(
                             course => new SchoolClassCourse
                             {
                                 SchoolClassId = schoolClass.Id,
                                 SchoolClass = schoolClass,
                                 CourseId = course.Id,
                                 Course = course,
                                 CreatedBy = user
                             }))
                    // Add the association to the SchoolClass's SchoolClassCourses collection
                    dataContextInUse.SchoolClassCourses.Add(schoolClassCourse);

            // ------------------------------------------------------------------ //
            Console.WriteLine("debug zone...", Color.Red);

            // Save the changes to the database
            await dataContextInUse.SaveChangesAsync();
        }


        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...", Color.Red);
    }
}