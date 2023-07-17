using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.Courses;
using SchoolProject.Web.Data.Entities.ExtraTables;
using SchoolProject.Web.Data.Entities.School;
using Serilog;

namespace SchoolProject.Web.Data.Entities.Teachers;

public static class Teachers
{
    public static List<Teacher> TeachersList { get; set; } = new();

    public static readonly Dictionary<int, Teacher> TeachersDictionary = new();


    public static void AddTeacher(
        string firstName,
        string lastName,
        string address,
        string postalCode,
        City city,
        Country country,
        string mobilePhone,
        string email,
        bool active,
        Genre genre,
        DateTime dateOfBirth,
        string identificationNumber,
        DateTime expirationDateIn,
        string taxIdentificationNumber,
        Country countryOfNationality,
        Nationality nationality,
        Country birthplace,
        Guid profilePhotoId,
        List<Course> courses
    )
    {
        User user = AuthenticatedUser.GetUser().Result ??
                    throw new InvalidOperationException();

        var teacher = new Teacher
        {
            Id = TeachersList.Count + 1,
            IdGuid = Guid.NewGuid(),
            FirstName = firstName,
            LastName = lastName,
            Address = address,
            PostalCode = postalCode,
            City = city,
            Country = country,
            MobilePhone = mobilePhone,
            Email = email,
            Active = active,
            Genre = genre,
            DateOfBirth = dateOfBirth,
            IdentificationNumber = identificationNumber,
            ExpirationDateIdentificationNumber = expirationDateIn,
            TaxIdentificationNumber = taxIdentificationNumber,
            CountryOfNationality = countryOfNationality,
            Birthplace = birthplace,
            EnrollDate = DateTime.Now.ToUniversalTime(),
            CreatedAt = DateTime.Now.ToUniversalTime(),
            CreatedBy = user,
            // UpdatedAt = DateTime.Now.ToUniversalTime(),
            // UpdatedBy = user,
            User = new User
            {
                FirstName = firstName,
                LastName = lastName,
                UserName = $"{firstName}.{lastName}@mail.pt",
                WasDeleted = false
            },
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
        City city,
        string mobilePhone,
        string email,
        bool active,
        Genre genre,
        DateTime dateOfBirth,
        string identificationNumber,
        DateTime expirationDateIn,
        string taxIdentificationNumber,
        Nationality nationality,
        Country birthplace,
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

        teacher.FirstName = firstname;
        teacher.LastName = lastName;
        teacher.Address = address;
        teacher.PostalCode = postalCode;
        teacher.City = city;
        teacher.MobilePhone = mobilePhone;
        teacher.Email = email;
        teacher.Active = active;
        teacher.Genre = genre;
        teacher.DateOfBirth = dateOfBirth;
        teacher.IdentificationNumber = identificationNumber;
        teacher.ExpirationDateIdentificationNumber = expirationDateIn;
        teacher.TaxIdentificationNumber = taxIdentificationNumber;

        teacher.Birthplace = birthplace;
        teacher.ProfilePhotoId = profilePhotoId;

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
        City city,
        string phone,
        string email,
        bool active,
        Genre genre,
        DateTime dateOfBirth,
        string identificationNumber,
        DateTime expirationDateIn,
        string taxIdentificationNumber,
        Nationality nationality,
        Country birthplace,
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

        teachers = TeachersList
            .Where(a => a.Nationality == nationality)
            .ToList();

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


    public static IEnumerable<Teacher> FilterTeachers(
        string name, string lastName, string address, string postalCode,
        City city, string phone, string email, bool active, Genre genre,
        DateTime dateOfBirth, string identificationNumber,
        DateTime expirationDateIn, string taxIdentificationNumber,
        Nationality nationality, Country birthplace, Guid profilePhotoId,
        int totalWorkHours)
    {
        var query = TeachersList.AsQueryable();

        if (!string.IsNullOrWhiteSpace(name))
            query = query.Where(a => a.FirstName == name);

        if (!string.IsNullOrWhiteSpace(lastName))
            query = query.Where(a => a.LastName == lastName);

        if (!string.IsNullOrWhiteSpace(address))
            query = query.Where(a => a.Address == address);

        if (!string.IsNullOrWhiteSpace(postalCode))
            query = query.Where(a => a.PostalCode == postalCode);

        query = query.Where(a => a.City == city);

        if (!string.IsNullOrWhiteSpace(phone))
            query = query.Where(a => a.MobilePhone == phone);

        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(a => a.Email == email);

        query = query.Where(a => a.Active == active);

        query = query.Where(a => a.Genre == genre);

        if (dateOfBirth != default)
            query = query.Where(a => a.DateOfBirth == dateOfBirth);

        if (!string.IsNullOrWhiteSpace(identificationNumber))
            query = query.Where(a =>
                a.IdentificationNumber == identificationNumber);

        if (expirationDateIn != default)
            query = query.Where(a =>
                a.ExpirationDateIdentificationNumber == expirationDateIn);

        if (!string.IsNullOrWhiteSpace(taxIdentificationNumber))
            query = query.Where(a =>
                a.TaxIdentificationNumber == taxIdentificationNumber);

        query = query.Where(a => a.Nationality == nationality);

        query = query.Where(a => a.Birthplace == birthplace);

        if (profilePhotoId != Guid.Empty)
            query = query.Where(a => a.ProfilePhotoId == profilePhotoId);

        if (!int.IsNegative(totalWorkHours))
            query = query.Where(a => a.TotalWorkHours == totalWorkHours);


        return query.ToList();
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
            .Where(t => property
                .GetValue(t)?.Equals(convertedValue) == true)
            .ToList();
    }


    private static int GetLastIndex()
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
            // teacher.CalculateTotalWorkHours();
            // teacher.CountCourses();
            // teacher.CalculateWorkloadPerCourse();
            Log.Information(
                "Metrics for {TeacherFirstName}: " +
                "Total work hours = {TeacherTotalWorkHours}, " +
                "Course count = {TeacherCoursesCount}, ",
                teacher.FirstName,
                teacher.TotalWorkHours,
                teacher.CoursesCount);
        // $"Workload per course = {teacher.workloadPerCourse}.");
        Log.Information("Teacher metrics calculation completed");
    }


    private static string GetFullName(int id)
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