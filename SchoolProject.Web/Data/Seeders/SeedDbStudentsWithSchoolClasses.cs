using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.SchoolClasses;
using SchoolProject.Web.Data.EntitiesOthers;

namespace SchoolProject.Web.Data.Seeders;

public class SeedDbStudentsWithSchoolClasses
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
        var schoolClasses =
            await dataContextInUse.SchoolClasses.ToListAsync();
        schoolClasses.ToList();


        // ------------------------------------------------------------------ //
        // Get all students from the database
        var students =
            await dataContextInUse.Students.ToListAsync();
        students.ToList();


        // ------------------------------------------------------------------ //
        // Get all students from the database
        var schoolClassStudents =
            await dataContextInUse.SchoolClassStudents.ToListAsync();
        schoolClassStudents.ToList();


        // ------------------------------------------------------------------ //
        // Verifica se já há dados para popular a lista de school-classes dos estudantes
        if (!schoolClasses.Any() || schoolClassStudents.Any()) return;


        // ------------------------------------------------------------------ //
        // Create a random number generator
        var random = new Random();


        // Collect new associations in memory
        var newAssociations = new List<(int StudentId, int SchoolClassId)>();

        foreach (var student in students)
        {
            var numberOfSchoolClasses = random.Next(1, 4);

            for (var i = 0; i < numberOfSchoolClasses; i++)
            {
                var randomSchoolClass =
                    schoolClasses[random.Next(schoolClasses.Count)];

                // Check if the association already exists in the database
                if (!newAssociations.Contains(
                        (student.Id, randomSchoolClass.Id)))
                    newAssociations.Add((student.Id, randomSchoolClass.Id));
            }
        }


        foreach (var (studentId, schoolClassId) in newAssociations)
        {
            var schoolClass =
                schoolClasses.FirstOrDefault(sc => sc.Id == schoolClassId);
            var student = students.FirstOrDefault(s => s.Id == studentId);

            if (schoolClass == null || student == null) continue;

            var schoolClassStudent = new SchoolClassStudent
            {
                SchoolClassId = schoolClass.Id,
                SchoolClass = schoolClass,
                StudentId = student.Id,
                Student = student,
                CreatedBy = user,
            };

            _dataContextInUse.SchoolClassStudents.Add(schoolClassStudent);
        }


        // Save the changes to the database
        await dataContextInUse.SaveChangesAsync();
    }
}