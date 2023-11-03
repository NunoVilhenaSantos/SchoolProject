using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Disciplines;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Data.Seeders;

/// <summary>
/// </summary>
public class SeedDbTeachersWithDisciplines
{
    private static List<Discipline> _listOfDisciplinesFromDb;
    private static List<Teacher> _listOfTeachersFromDb;


    /// <summary>
    ///     code for fetching existing courses and disciplines
    /// </summary>
    /// <param name="dataContextInUse"></param>
    /// <param name="appUser"></param>
    public static async Task AddingData(
        // DataContextMsSql dataContextInUse, 
        DataContextMySql dataContextInUse, AppUser appUser
    )
    {
        Console.WriteLine("debug zone...");

        // ------------------------------------------------------------------ //

        await AddDataToDb(appUser, dataContextInUse);

        // ------------------------------------------------------------------ //

        Console.WriteLine("debug zone...");
    }


    private static async Task AddDataToDb(
        AppUser appUser, DataContextMySql dataContextInUse)
    {
        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");
        // ------------------------------------------------------------------ //


        // ------------------------------------------------------------------ //
        var existingDisciplines =
            await dataContextInUse.Disciplines.ToListAsync();
        _listOfDisciplinesFromDb = existingDisciplines.ToList();


        // ------------------------------------------------------------------ //
        var existingTeachers =
            await dataContextInUse.Teachers.ToListAsync();
        _listOfTeachersFromDb = existingTeachers.ToList();


        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");
        // ------------------------------------------------------------------ //


        // ------------------------------------------------------------------ //
        if (await dataContextInUse.TeacherDisciplines.AnyAsync()) return;


        // ------------------------------------------------------------------ //
        var random = new Random();


        foreach (var teacherDiscipline in
                 from discipline in _listOfDisciplinesFromDb
                 let teacher = _listOfTeachersFromDb[
                     random.Next(_listOfTeachersFromDb.Count)]
                 select new TeacherDiscipline
                 {
                     TeacherId = teacher.Id,
                     Teacher = teacher,
                     DisciplineId = discipline.Id,
                     Discipline = discipline,
                     CreatedById = appUser.Id,
                     CreatedBy = appUser
                 })

            // Add the TeacherCourse association to the context
            dataContextInUse.TeacherDisciplines.Add(teacherDiscipline);


        // -------------------------------------------------------------- //
        Console.WriteLine("debug zone...");


        // Save the changes to the database
        await dataContextInUse.SaveChangesAsync();


        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");
    }
}