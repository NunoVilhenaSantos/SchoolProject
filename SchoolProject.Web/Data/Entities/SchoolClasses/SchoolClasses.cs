using System.Diagnostics;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.School;

namespace SchoolProject.Web.Data.Entities.SchoolClasses;

public static class SchoolClasses
{
    #region Properties

    public static List<SchoolClass> SchoolClassesList { get; set; } = new();

    public static readonly Dictionary<int, SchoolClass>
        SchoolClassesDictionary = new();

    #endregion


    #region Methods

    public static void AddSchoolClass(
        int id, string classAcronym, string className,
        DateTime startDate, DateTime endDate,
        TimeSpan startHour, TimeSpan endHour,
        string location, string type, string area, int studentsCount,
        List<Course>? courses
    )
    {
        SchoolClassesList.Add(new SchoolClass
            {
                Id = id,
                ClassAcronym = classAcronym,
                ClassName = className,
                StartDate = startDate,
                EndDate = endDate,
                StartHour = startHour,
                EndHour = endHour,
                Location = location,
                Type = type,
                Area = area
                //StudentsCount = studentsCount,
                //CoursesList = courses
            }
        );
        SchoolDatabase.AddSchoolClass(SchoolClassesList[^1]);

        SchoolClassesList[^1].GetStudentsCount();
        SchoolClassesList[^1].GetWorkHourLoad();
    }


    public static string DeleteSchoolClass(int id)
    {
        var schoolClass =
            SchoolClassesList
                .FirstOrDefault(a => a.Id == id);

        if (schoolClass == null) return $"A turma {id} não existe!\n";

        var fullInfo = GetFullInfo(id);
        SchoolClassesList.Remove(schoolClass);
        return $"A turma {schoolClass.ClassName} com " +
               $"o {id} foi apagada!\n{fullInfo}";
    }


    public static string EditSchoolClass(
        int id, string classAcronym, string className,
        DateTime startDate, DateTime endDate,
        TimeSpan startHour, TimeSpan endHour,
        string location, string type, string area
    )
    {
        if (SchoolClassesList.Count < 1)
            return "A lista está vazia";

        var schoolClass =
            SchoolClassesList.FirstOrDefault(
                a => a.Id == id);

        if (schoolClass == null)
            return "A turma não existe!";

        SchoolClassesList.FirstOrDefault(
            a => a.Id == id)!.ClassAcronym = classAcronym;
        SchoolClassesList.FirstOrDefault(
            a => a.Id == id)!.ClassName = className;
        SchoolClassesList.FirstOrDefault(
            a => a.Id == id)!.StartDate = startDate;
        SchoolClassesList.FirstOrDefault(
            a => a.Id == id)!.EndDate = endDate;
        SchoolClassesList.FirstOrDefault(
            a => a.Id == id)!.StartHour = startHour;
        SchoolClassesList.FirstOrDefault(
            a => a.Id == id)!.EndHour = endHour;

        SchoolClassesList.FirstOrDefault(
            a => a.Id == id)!.Location = location;
        SchoolClassesList.FirstOrDefault(
            a => a.Id == id)!.Type = type;
        SchoolClassesList.FirstOrDefault(
            a => a.Id == id)!.Area = area;


        SchoolClassesList[^1].GetStudentsCount();
        SchoolClassesList[^1].GetWorkHourLoad();

        return "Turma alterada com sucesso";
    }

    public static List<SchoolClass> ConsultSchoolClasses(
        int? id, string classAcronym, string className,
        DateTime? startDate, DateTime? endDate,
        TimeSpan? startHour, TimeSpan? endHour,
        string location, string type, string area,
        int? studentsCount,
        List<Course>? courses
    )
    {
        var query = SchoolClassesList.AsQueryable();

        if (id.HasValue) query = query.Where(a => a.Id == id.Value);
        if (!string.IsNullOrWhiteSpace(classAcronym))
            query = query.Where(a => a.ClassAcronym == classAcronym);
        if (!string.IsNullOrWhiteSpace(className))
            query = query.Where(a => a.ClassName == className);
        if (startDate.HasValue)
            query = query.Where(a => a.StartDate == startDate.Value);
        if (endDate.HasValue && endDate.Value > startDate)
            query = query.Where(a => a.EndDate == endDate.Value);
        if (startHour.HasValue)
            query = query.Where(a => a.StartHour == startHour.Value);
        if (endHour.HasValue && endHour.Value > startHour)
            query = query.Where(a => a.EndHour == endHour.Value);
        if (!string.IsNullOrWhiteSpace(location))
            query = query.Where(a => a.Location == location);
        if (!string.IsNullOrWhiteSpace(type))
            query = query.Where(a => a.Type == type);
        if (!string.IsNullOrWhiteSpace(area))
            query = query.Where(a => a.Area == area);
        if (studentsCount.HasValue && studentsCount >= 0)
            query = query.Where(a => a.StudentsCount == studentsCount.Value);

        var schoolClasses = query.ToList();
        return schoolClasses;
    }


    public static List<SchoolClass> ConsultSchoolClasses(
        string selectedProperty, object selectedValue)
    {
        var property = typeof(SchoolClass).GetProperty(selectedProperty);
        if (property == null) return new List<SchoolClass>();

        var propertyType = property.PropertyType;
        object convertedValue;
        try
        {
            convertedValue = Convert.ChangeType(selectedValue, propertyType);
        }
        catch (InvalidCastException ex)
        {
            // Handle invalid cast exception
            Console.WriteLine($"Invalid cast: {ex.Message}");
            return new List<SchoolClass>();
        }
        catch (FormatException ex)
        {
            // Handle format exception
            Console.WriteLine($"Invalid format: {ex.Message}");
            return new List<SchoolClass>();
        }

        return SchoolClassesList
            .Where(schoolClass =>
                property.GetValue(schoolClass)
                    ?.Equals(convertedValue) ==
                true)
            .ToList();
    }


    private static int GetLastIndex()
    {
        // handle the case where the collection is empty
        // return StudentsList[^1].IdStudent;
        // return GetLastIndex();
        var lastSchoolClasses = SchoolClassesList.LastOrDefault();
        if (lastSchoolClasses != null)
            return lastSchoolClasses.Id;
        return -1;
    }


    public static int GetLastId()
    {
        var lastSchoolClasses = SchoolClassesList.LastOrDefault();
        return lastSchoolClasses?.Id ?? GetLastIndex();
        /*
        return lastSchoolClasses != null
            ? lastSchoolClasses.Id
            : GetLastIndex();
        // handle the case where the collection is empty
        // return StudentsList[^1].IdStudent;
        // return GetLastIndex();
        */
    }


    private static string GetFullName(int id)
    {
        if (SchoolClassesList.Count < 1)
            return "A lista está vazia";

        var schoolClass =
            SchoolClassesList.FirstOrDefault(
                a => a.Id == id);

        if (schoolClass == null)
            return "A turma não existe!";

        return $"{schoolClass.Id,5} | " +
               $"{schoolClass.ClassAcronym} " +
               $"{schoolClass.ClassName}";
    }


    private static string GetFullInfo(int id)
    {
        if (SchoolClassesList.Count < 1)
            return "A lista está vazia";

        var schoolClass =
            SchoolClassesList.FirstOrDefault(
                a => a.Id == id);

        if (schoolClass == null)
            return "A turma não existe!";

        return $"{GetFullName(id)} | " +
               $"{schoolClass.Type} - {schoolClass.Area}";
    }


    public static void ToObtainValuesForCalculatedFields()
    {
        if (SchoolClassesList.Count < 1) return;

        foreach (var schoolClass in SchoolClassesList)
        {
            var coursesList =
                SchoolDatabase
                    .GetCoursesForSchoolClass(schoolClass.Id);

            if (!coursesList.Any()) continue;

            // use a set to ensure uniqueness of student IDs
            var students = new HashSet<string>();
            var coursesCount = 0;
            var workHourLoad = 0;
            decimal classTotal = 0;
            decimal highestGrade = 0;
            var lowestGrade = decimal.MaxValue;

            foreach (var course in coursesList.Where(_ => true))
            {
                coursesCount++;
                workHourLoad += course.WorkLoad;

                var courseEnrollments =
                    Enrollments.Enrollments.ListEnrollments?
                        .Where(e => e.Course.Id == course.Id)
                        .ToList();

                if (courseEnrollments == null || !courseEnrollments.Any())
                    continue;

                var courseGrades = courseEnrollments
                    .Where(e => e.Grade.HasValue)
                    .Select(e =>
                    {
                        Debug.Assert(e.Grade != null, "e.Grade != null");
                        return e.Grade.Value;
                    })
                    .ToList();

                if (courseGrades.Any())
                {
                    classTotal += courseGrades.Average();
                    highestGrade =
                        Math.Max(highestGrade, courseGrades.Max());
                    lowestGrade =
                        Math.Min(lowestGrade, courseGrades.Min());
                }

                students.UnionWith(courseEnrollments
                    .Select(e => e.Student.Id.ToString()));
            }

            schoolClass.CoursesCount = coursesCount;
            schoolClass.WorkHourLoad = workHourLoad;
            schoolClass.StudentsCount = students.Count;
            schoolClass.ClassAverage =
                coursesCount > 0 ? classTotal / coursesCount : 0;
            schoolClass.HighestGrade = highestGrade;
            schoolClass.LowestGrade = lowestGrade;
        }
    }

    #endregion
}