using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.School;
using Serilog;

namespace SchoolProject.Web.Data.Entities.Teachers;

public static class Teachers
{
    public static readonly Dictionary<int, Teacher> TeachersDictionary = new();
    public static List<Teacher> TeachersList { get; set; } = new();


    public static void AddTeacher(
        int id,
        string firstName,
        string lastName,
        string address,
        string postalCode,
        string city,
        string mobilePhone,
        string email,
        bool active,
        string genre,
        DateTime dateOfBirth,
        string identificationNumber,
        DateTime expirationDateIn,
        string taxIdentificationNumber,
        string nationality,
        string birthplace,
        Guid profilePhotoId,
        int coursesCount,
        int totalWorkHours,
        List<Course> courses
    )
    {
        var teacher = new Teacher
        {
            //Id = id,
            FirstName = firstName,
            LastName = lastName,
            Address = address,
            PostalCode = postalCode,
            City = city,
            MobilePhone = mobilePhone,
            Email = email,
            Active = active,
            Genre = genre,
            DateOfBirth = dateOfBirth,
            IdentificationNumber = identificationNumber,
            ExpirationDateIdentificationNumber = expirationDateIn,
            TaxIdentificationNumber = taxIdentificationNumber,
            Nationality = nationality,
            Birthplace = birthplace,
            ProfilePhotoId = profilePhotoId,
            CoursesCount = coursesCount,
            TotalWorkHours = totalWorkHours
            //Courses = courses
        };
        TeachersList.Add(teacher);

        SchoolDatabase.AddTeacher(TeachersList[^1]);
    }


    public static string EditTeacher(
        int id,
        string firstname,
        string lastName,
        string address,
        string postalCode,
        string city,
        string mobilePhone,
        string email,
        bool active,
        string genre,
        DateTime dateOfBirth,
        string identificationNumber,
        DateTime expirationDateIn,
        string taxIdentificationNumber,
        string nationality,
        string birthplace,
        Guid profilePhotoId,
        int coursesCount,
        int totalWorkHours,
        List<Course> courses
    )
    {
        if (TeachersList.Count < 1) return "Lista está vazia";

        var teacher =
            TeachersList.FirstOrDefault(x => x.Id == id);

        if (teacher == null) return "Professor(a) não existe";

        TeachersList.FirstOrDefault(a => a.Id == id)!.FirstName = firstname;
        TeachersList.FirstOrDefault(a => a.Id == id)!.LastName = lastName;
        TeachersList.FirstOrDefault(a => a.Id == id)!.Address = address;
        TeachersList.FirstOrDefault(a => a.Id == id)!.PostalCode = postalCode;
        TeachersList.FirstOrDefault(a => a.Id == id)!.City = city;
        TeachersList.FirstOrDefault(a => a.Id == id)!.MobilePhone = mobilePhone;
        TeachersList.FirstOrDefault(a => a.Id == id)!.Email = email;
        TeachersList.FirstOrDefault(a => a.Id == id)!.Active = active;
        TeachersList.FirstOrDefault(a => a.Id == id)!.Genre = genre;
        TeachersList.FirstOrDefault(a => a.Id == id)!.DateOfBirth = dateOfBirth;
        TeachersList.FirstOrDefault(a => a.Id == id)!.IdentificationNumber =
            identificationNumber;
        TeachersList.FirstOrDefault(a => a.Id == id)!
            .ExpirationDateIdentificationNumber = expirationDateIn;
        TeachersList.FirstOrDefault(a => a.Id == id)!.TaxIdentificationNumber =
            taxIdentificationNumber;
        TeachersList.FirstOrDefault(a => a.Id == id)!.Nationality = nationality;
        TeachersList.FirstOrDefault(a => a.Id == id)!.Birthplace = birthplace;
        TeachersList.FirstOrDefault(a => a.Id == id)!.ProfilePhotoId =
            profilePhotoId;
        TeachersList.FirstOrDefault(a => a.Id == id)!.TotalWorkHours =
            totalWorkHours;
        TeachersList.FirstOrDefault(a => a.Id == id)!.CoursesCount =
            coursesCount;

        TeachersList[id].CountCourses();
        TeachersList[id].GetTotalWorkHourLoad();

        return "Professor(a) alterado(a) com sucesso";
    }


    public static string RemoveTeacher(int id)
    {
        var teacher = TeachersList.FirstOrDefault(x => x.Id == id);

        if (teacher == null) return "Professor(a) não existe";

        TeachersList.Remove(teacher);
        return "Professor(a) removido(a) com sucesso";
    }


    public static List<Teacher> ConsultTeacher(
        int id,
        string name,
        string lastName,
        string address,
        string postalCode,
        string city,
        string phone,
        string email,
        bool active,
        string genre,
        DateTime dateOfBirth,
        string identificationNumber,
        DateTime expirationDateIn,
        string taxIdentificationNumber,
        string nationality,
        string birthplace,
        Guid profilePhotoId,
        int totalWorkHours,
        List<Course> courses
    )
    {
        var teachers = TeachersList;


        if (!string.IsNullOrWhiteSpace(name))
            teachers = TeachersList
                .Where(a => a.FirstName == name).ToList();
        if (!string.IsNullOrWhiteSpace(lastName))
            teachers = TeachersList
                .Where(a => a.LastName == lastName).ToList();
        if (!string.IsNullOrWhiteSpace(address))
            teachers = TeachersList
                .Where(a => a.Address == address).ToList();
        if (!string.IsNullOrWhiteSpace(postalCode))
            teachers = TeachersList
                .Where(a => a.PostalCode == postalCode).ToList();
        if (!string.IsNullOrWhiteSpace(city))
            teachers = TeachersList
                .Where(a => a.City == city).ToList();
        if (!string.IsNullOrWhiteSpace(phone))
            teachers = TeachersList
                .Where(a => a.MobilePhone == phone).ToList();
        if (!string.IsNullOrWhiteSpace(email))
            teachers = TeachersList
                .Where(a => a.Email == email).ToList();
        teachers = TeachersList
            .Where(a => a.Active == active).ToList();
        if (!string.IsNullOrWhiteSpace(genre))
            teachers = TeachersList
                .Where(a => a.Genre == genre).ToList();
        if (dateOfBirth != default)
            teachers = TeachersList
                .Where(a => a.DateOfBirth == dateOfBirth)
                .ToList();
        if (!string.IsNullOrWhiteSpace(identificationNumber))
            teachers = TeachersList
                .Where(a =>
                    a.IdentificationNumber == identificationNumber)
                .ToList();
        if (expirationDateIn != default)
            teachers = TeachersList
                .Where(a =>
                    a.ExpirationDateIdentificationNumber == expirationDateIn)
                .ToList();
        if (!string.IsNullOrWhiteSpace(taxIdentificationNumber))
            teachers = TeachersList
                .Where(a =>
                    a.TaxIdentificationNumber == taxIdentificationNumber)
                .ToList();
        if (!string.IsNullOrWhiteSpace(nationality))
            teachers = TeachersList
                .Where(a => a.Nationality == nationality)
                .ToList();
        if (!string.IsNullOrWhiteSpace(birthplace))
            teachers = TeachersList
                .Where(a => a.Birthplace == birthplace)
                .ToList();
        if (profilePhotoId != Guid.Empty)
            teachers = TeachersList
                .Where(a => a.ProfilePhotoId == profilePhotoId)
                .ToList();
        if (!int.IsNegative(totalWorkHours))
            teachers = TeachersList
                .Where(a => a.TotalWorkHours == totalWorkHours)
                .ToList();

        return teachers;
    }


    public static List<Teacher> ConsultTeachers(
        string selectedProperty, object selectedValue)
    {
        var property = typeof(Teacher).GetProperty(selectedProperty);
        if (property == null) return new List<Teacher>();

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
            return new List<Teacher>();
        }
        catch (FormatException ex)
        {
            // Handle format exception
            Console.WriteLine($"Invalid format: {ex.Message}");
            return new List<Teacher>();
        }

        return TeachersList
            .Where(t => property.GetValue(t)?.Equals(convertedValue) == true)
            .ToList();
    }


    public static int GetLastIndex()
    {
        var lastTeachers = TeachersList.LastOrDefault();
        if (lastTeachers != null)
            return lastTeachers.Id;
        return -1;
    }


    public static int GetLastId()
    {
        var lastTeachers = TeachersList.LastOrDefault();
        return lastTeachers?.Id ?? GetLastIndex();
        /*
        return lastTeachers != null
            ? lastTeachers.Id
            : GetLastIndex();
        // handle the case where the collection is empty
        // return StudentsList[^1].IdStudent;
        // return GetLastIndex();
        */
    }


    public static void CalculateTeacherMetrics()
    {
        if (TeachersList.Count < 1)
            Log.Warning("No teachers found in the directory");

        foreach (var teacher in TeachersList)
        {
            teacher.CalculateTotalWorkHours();
            teacher.CountCourses();
            //teacher.CalculateWorkloadPerCourse();

            Log.Information(
                $"Metrics for {teacher.FirstName}: " +
                $"Total work hours = {teacher.TotalWorkHours}, " +
                $"Course count = {teacher.CoursesCount}, ");
            // $"Workload per course = {teacher.workloadPerCourse}.");
        }

        Log.Information("Teacher metrics calculation completed");
    }


    public static string GetFullName(int id)
    {
        if (TeachersList.Count < 1)
            return "A lista está vazia";

        var teacher =
            TeachersList.FirstOrDefault(
                a => a.Id == id);

        if (teacher == null)
            return "A turma não existe!";

        return $"{teacher.Id,5} | " +
               $"{teacher.FirstName} " +
               $"{teacher.LastName}";
    }


    public static string GetFullInfo(int id)
    {
        if (TeachersList.Count < 1)
            return "A lista está vazia";

        var teacher =
            TeachersList.FirstOrDefault(
                a => a.Id == id);

        if (teacher == null)
            return "A turma não existe!";

        return $"{GetFullName(id)} | " +
               $"{teacher.Genre} - {teacher.City}";
    }
}