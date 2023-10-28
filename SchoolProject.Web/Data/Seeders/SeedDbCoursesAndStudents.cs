using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.Enrollments;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Users;

namespace SchoolProject.Web.Data.Seeders;

/// <summary>
/// </summary>
public class SeedDbCoursesAndStudents
{
    // private static DataContextMsSql _dataContextMsSql;
    // private static DataContextMsSql _dataContextInUse;
    private static readonly DataContextMySql _dataContextInUse;


    // public static void Initialize(DataContextMsSql dataContextMsSql)
    // {
    //     _dataContextMsSql = dataContextMsSql;
    // }


    public static async Task AddingData(AppUser appUser,
        DataContextMySql dataContextInUse)
    {
        // list of courses with disciplines
        var coursesWithDisciplines = await dataContextInUse.Courses
            .Include(course => course.CourseDisciplines)
            .ThenInclude(courseDisciplines => courseDisciplines.Discipline)
            .ToListAsync();


        // list of students
        var students = await dataContextInUse.Students.ToListAsync();

        // Get all students from the database
        var courseStudentsList =
            await dataContextInUse.CourseStudents.ToListAsync();


        // Get all students from the database
        var studentDisciplinesList =
            await dataContextInUse.StudentDisciplines.ToListAsync();


        // Get all students from the database
        var enrollmentsList =
            await dataContextInUse.Enrollments.ToListAsync();


        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");
        // ------------------------------------------------------------------ //


        if (studentDisciplinesList.Any() || enrollmentsList.Any())
        {
            Console.WriteLine("debug zone...");

            Console.WriteLine(
                $"coursesWithDisciplines has {coursesWithDisciplines.Count} records");
            Console.WriteLine(
                $"courseStudentsList has {courseStudentsList.Count} records");

            Console.WriteLine(
                $"studentDisciplinesList has {studentDisciplinesList.Count} records");
            Console.WriteLine(
                $"enrollmentsList has {studentDisciplinesList.Count} records");

            return;
        }


        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");
        // ------------------------------------------------------------------ //


        // ------------------------------------------------------------------ //
        // Verifica se já há dados para popular a lista de estudantes com as turmas e as suas disciplinas
        // ------------------------------------------------------------------ //
        // Check if there is already data to populate the list of students with classes and their subjects
        if (!coursesWithDisciplines.Any() || courseStudentsList.Any()) return;


        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");
        // ------------------------------------------------------------------ //


        // random number generator
        var random = new Random();


        // Collect new associations in memory
        var courseStudentDictionary =
            new Dictionary<(int CourseId, int StudentId), CourseStudent>();

        var enrollmentDictionary =
            new Dictionary<(int StudentId, int DisciplineId), Enrollment>();

        var studentDisciplineDictionary =
            new Dictionary<(int StudentId, int DisciplineId),
                StudentDiscipline>();


        // generates the associations with random data
        foreach (var student in students)
        {
            var numberOfCourses = random.Next(3);

            for (var i = 0; i < numberOfCourses; i++)
            {
                var randomCourse =
                    coursesWithDisciplines[
                        random.Next(coursesWithDisciplines.Count)];

                var courseStudentKey = (randomCourse.Id, student.Id);

                if (!courseStudentDictionary.ContainsKey(courseStudentKey))
                {
                    var courseStudent = new CourseStudent
                    {
                        CourseId = randomCourse.Id,
                        Course = randomCourse,
                        StudentId = student.Id,
                        Student = student,
                        CreatedBy = appUser,
                        CreatedById = appUser.Id,
                    };

                    courseStudentDictionary.Add(courseStudentKey,
                        courseStudent);
                }

                foreach (var courseDiscipline in randomCourse.CourseDisciplines
                             .Where(courseDiscipline =>
                                 courseDiscipline.Discipline != null))
                {
                    var enrollmentKey =
                        (student.Id, courseDiscipline.Discipline.Id);

                    var studentDisciplineKey =
                        (student.Id, courseDiscipline.Discipline.Id);

                    if (!enrollmentDictionary.ContainsKey(enrollmentKey))
                    {
                        var enrollment = new Enrollment
                        {
                            // StudentId = student.Id,
                            Student = student,
                            // DisciplineId = courseDiscipline.Discipline.Id,
                            Discipline = courseDiscipline.Discipline,
                            Absences = 0,
                            CreatedBy = appUser,
                            CreatedById = appUser.Id,
                        };

                        enrollmentDictionary.Add(enrollmentKey, enrollment);
                    }

                    if (studentDisciplineDictionary.ContainsKey(
                            studentDisciplineKey)) continue;

                    var studentDiscipline = new StudentDiscipline
                    {
                        // StudentId = student.Id,
                        Student = student,
                        // DisciplineId = courseDiscipline.Discipline.Id,
                        Discipline = courseDiscipline.Discipline,
                        CreatedBy = appUser,
                        CreatedById = appUser.Id
                    };

                    studentDisciplineDictionary.Add(studentDisciplineKey,
                        studentDiscipline);
                }
            }
        }

        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");
        // ------------------------------------------------------------------ //

        await using var transaction =
            await dataContextInUse.Database.BeginTransactionAsync();

        try
        {
            dataContextInUse.CourseStudents.AddRange(courseStudentDictionary
                .Values);

            dataContextInUse.Enrollments.AddRange(enrollmentDictionary.Values);

            dataContextInUse.StudentDisciplines.AddRange(
                studentDisciplineDictionary.Values);

            await dataContextInUse.SaveChangesAsync();

            await transaction.CommitAsync();

            // -------------------------------------------------------------- //
            Console.WriteLine("debug zone...");
            // -------------------------------------------------------------- //
        }
        catch (Exception ex)
        {
            // Trate as exceções ou reverta a transação, se necessário.
            await transaction.RollbackAsync();

            throw new Exception(
                "Erro ao salvar as alterações no banco de dados: " +
                ex.Message, ex);

            // -------------------------------------------------------------- //
            Console.WriteLine("debug zone...");
            // -------------------------------------------------------------- //
        }

        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone...");
        // ------------------------------------------------------------------ //
    }
}