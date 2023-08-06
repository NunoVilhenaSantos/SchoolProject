﻿using System.Drawing;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Seeders;

/// <summary>
///    This class is used to seed the database with some initial data.
/// </summary>
public class SeedDbSchoolClassesWithCourses
{
    private static DataContextMsSql _dataContextMsSql;

    private static List<Course> _listOfCoursesToAdd = new();
    private static List<SchoolClass> _listOfSchoolClassesToAdd = new();


    public static void Initialize(DataContextMsSql dataContextMsSql)
    {
        _dataContextMsSql = dataContextMsSql;
    }


    // Your existing code for fetching existing school classes and courses
    public static async Task AddingData(
        DataContextMsSql dataContextMsSql, User user)
    {
        Console.WriteLine("debug zone...");


        // ------------------------------------------------------------------ //
        var existingCourses =
            await dataContextMsSql.Courses.ToListAsync();

        var existingSchoolClasses =
            await dataContextMsSql.SchoolClasses
                // To Ensure Courses are loaded for each SchoolClass
                .Include(sc => sc.Courses)
                .ToListAsync();

        _listOfCoursesToAdd = existingCourses.ToList();
        _listOfSchoolClassesToAdd = existingSchoolClasses.ToList();

        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");

        // Loop through each school class
        foreach (var schoolClass in _listOfSchoolClassesToAdd)
        {
            // Check if Courses is null or empty before iterating
            if (schoolClass.Courses != null && schoolClass.Courses.Any())
            {
                // Loop through each course associated with the school class
                foreach (var course in schoolClass.Courses)
                {
                    // Create a new SchoolClassCourse to represent the association
                    var schoolClassCourse = new SchoolClassCourse
                    {
                        SchoolClassId = schoolClass.Id,
                        SchoolClass = schoolClass,
                        CourseId = course.Id,
                        Course = course,
                        CreatedBy = user,
                    };

                    // Add the association to the SchoolClass's SchoolClassCourses collection
                    dataContextMsSql.SchoolClassCourses.Add(schoolClassCourse);
                }
            }

            // ------------------------------------------------------------------ //
            Console.WriteLine("debug zone...", Color.Red);

            // Save the changes to the database
            await _dataContextMsSql.SaveChangesAsync();
        }


        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...", Color.Red);
    }
}