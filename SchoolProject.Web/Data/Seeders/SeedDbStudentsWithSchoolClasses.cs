using SchoolProject.Web.Data.DataContexts.MySQL;

namespace SchoolProject.Web.Data.Seeders;

public class SeedDbStudentsWithSchoolClasses
{
    // private static DataContextMsSql _dataContextMsSql;
    // private static DataContextMsSql _dataContextInUse;
    private static DataContextMySql _dataContextInUse;


    // public static void Initialize(DataContextMsSql dataContextMsSql)
    // {
    //     _dataContextMsSql = dataContextMsSql;
    // }

    public static async Task AddingData(DataContextMySql dataContextInUse)
    {
        var _dataContextInUse = dataContextInUse;

        // Get all school classes from the database
        var schoolClasses =
            await dataContextInUse.SchoolClasses.ToListAsync();

        // Get all students from the database
        var students =
            await dataContextInUse.Students.ToListAsync();

        // Verifica se jhá dados para popular a lista de schoolclasses dos estudantes
        if (!HasExistingData())
            // Decide como proceder quando já existem dados no banco de dados
            // Você pode optar por não executar o processo de seed novamente ou atualizar os dados existentes.
            // Por exemplo, você pode gerar um log ou mostrar uma mensagem ao usuário informando que os dados já existem.
            return;

        // ------------------------------------------------------------------ //

        // Create a random number generator
        var random = new Random();

        // Assign school classes to students
        // (assuming each student is associated with multiple school classes)
        foreach (var student in students)
        {
            if (student.SchoolClasses != null ||
                student.SchoolClasses.Count >= 0) continue;

            // Assign 1 to 3 random school classes to each student
            var numberOfSchoolClasses = random.Next(1, 4);

            for (var i = 0; i < numberOfSchoolClasses; i++)
            {
                var randomSchoolClass =
                    schoolClasses[random.Next(schoolClasses.Count)];

                // Add the school class to the Student's SchoolClasses collection
                student.SchoolClasses.Add(randomSchoolClass);

                // Add the student to the SchoolClass's Students collection
                randomSchoolClass.Students.Add(student);
            }
        }


        // Save the changes to the database
        await dataContextInUse.SaveChangesAsync();
    }


    private static bool HasExistingData()
    {
        var studentsCount = _dataContextInUse.Students?.Count();
        var schoolClassesCount = _dataContextInUse.SchoolClasses?.Count();

        // Verifica se existem registros nas tabelas Students e SchoolClasses
        return studentsCount > 0 && schoolClassesCount > 0;
    }
}