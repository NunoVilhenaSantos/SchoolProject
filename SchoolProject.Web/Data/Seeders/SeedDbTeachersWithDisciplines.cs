﻿using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Data.Seeders;

public class SeedDbTeachersWithDisciplines
{
    private static List<Discipline> _listOfCoursesFromDb;
    private static List<Teacher> _listOfTeachersFromDb;


    public static async Task AddingData(
        // DataContextMsSql dataContextInUse, 
        DataContextMySql dataContextInUse,
        AppUser appUser
    )
    {
        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");

        await AddDataToDb(appUser, dataContextInUse);

        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");
    }


    private static async Task AddDataToDb(AppUser appUser,
        DataContextMySql dataContextInUse)
    {
        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");


        // ------------------------------------------------------------------ //
        var existingCourses =
            await dataContextInUse.Disciplines.ToListAsync();
        _listOfCoursesFromDb = existingCourses.ToList();


        // ------------------------------------------------------------------ //
        var existingTeachers =
            await dataContextInUse.Teachers.ToListAsync();
        _listOfTeachersFromDb = existingTeachers.ToList();


        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");

        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");
        if (await dataContextInUse.TeacherCourses.AnyAsync()) return;


        // ------------------------------------------------------------------ //
        var random = new Random();

        foreach (var teacherCourse in from course in _listOfCoursesFromDb
                 let teacher = _listOfTeachersFromDb[
                     random.Next(_listOfTeachersFromDb.Count)]
                 select new TeacherCourse
                 {
                     TeacherId = teacher.Id,
                     Teacher = teacher,
                     CourseId = course.Id,
                     Course = course,
                     CreatedBy = appUser,
                     CreatedById = appUser.Id,
                 })
            // Add the TeacherCourse association to the context
            dataContextInUse.TeacherCourses.Add(teacherCourse);

        // -------------------------------------------------------------- //
        Console.WriteLine("debug zone...");


        // Save the changes to the database
        await dataContextInUse.SaveChangesAsync();


        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");
    }
}