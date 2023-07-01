using SchoolProject.Web.Data.Entities.School;
using Serilog;

namespace SchoolProject.Web.Data.Entities.Students;

public class Students
{
    #region Properties

    public static List<Student> StudentsList { get; set; } = new();
    public static readonly Dictionary<int, Student> StudentsDictionary = new();

    #endregion


    #region Methods

    public static void AddStudent(
        int id,
        string firstName,
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
        int courseCount,
        int totalWorkHours,
        DateTime enrollDate
    )
    {
        StudentsList.Add(new Student
            {
                //Id = id,
                FirstName = firstName,
                LastName = lastName,
                Address = address,
                PostalCode = postalCode,
                City = city,
                MobilePhone = phone,
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
                CoursesCount = courseCount,
                TotalWorkHours = totalWorkHours,
                EnrollDate = enrollDate
            }
        );
        SchoolDatabase.AddStudent(StudentsList.LastOrDefault());
        Console.WriteLine("Debugging");
    }


    public static string DeleteStudent(int id)
    {
        var student =
            StudentsList.FirstOrDefault(a => a.Id == id);

        if (student == null)
            return "O estudante não existe!";

        StudentsList.Remove(student);
        return "O estudante foi apagado!";
    }


    public static string EditStudent(
        int id,
        string firstName,
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
        Guid photo,
        int courseCount,
        int totalWorkHours,
        DateTime enrollmentDate
    )
    {
        if (StudentsList.Count < 1) return "A lista está vazia";

        var student = StudentsList.FirstOrDefault(s => s.Id == id);

        if (student == null)
            return "O estudante não existe!";

        StudentsList.FirstOrDefault(a => a.Id == id)!.FirstName = firstName;
        StudentsList.FirstOrDefault(a => a.Id == id)!.LastName = lastName;
        StudentsList.FirstOrDefault(a => a.Id == id)!.Address = address;
        StudentsList.FirstOrDefault(a => a.Id == id)!.PostalCode = postalCode;
        StudentsList.FirstOrDefault(a => a.Id == id)!.City = city;
        StudentsList.FirstOrDefault(a => a.Id == id)!.MobilePhone = phone;
        StudentsList.FirstOrDefault(a => a.Id == id)!.Email = email;
        StudentsList.FirstOrDefault(a => a.Id == id)!.Active = active;
        StudentsList.FirstOrDefault(a => a.Id == id)!.Genre = genre;
        StudentsList.FirstOrDefault(a => a.Id == id)!.DateOfBirth = dateOfBirth;
        StudentsList.FirstOrDefault(a => a.Id == id)!
            .IdentificationNumber = identificationNumber;
        StudentsList.FirstOrDefault(a => a.Id == id)!
            .ExpirationDateIdentificationNumber = expirationDateIn;
        StudentsList.FirstOrDefault(a => a.Id == id)!
            .TaxIdentificationNumber = taxIdentificationNumber;
        StudentsList.FirstOrDefault(a => a.Id == id)!.Nationality = nationality;
        StudentsList.FirstOrDefault(a => a.Id == id)!.Birthplace = birthplace;
        StudentsList.FirstOrDefault(a => a.Id == id)!.ProfilePhotoId = photo;
        StudentsList.FirstOrDefault(a => a.Id == id)!.TotalWorkHours =
            totalWorkHours;
        StudentsList.FirstOrDefault(a => a.Id == id)!.EnrollDate =
            enrollmentDate;

        StudentsList[id].CalculateTotalWorkHours();
        StudentsList[id].CountCourses();

        return "Estudante alterado com sucesso";
    }

    public static List<Student> ConsultStudent(
        int id,
        string firstName,
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
        Guid photo,
        int totalWorkHours,
        DateTime enrollmentDate
    )
    {
        IEnumerable<Student> query = StudentsList;

        if (id != 0)
            query = query.Where(a => a.Id == id);
        if (!string.IsNullOrWhiteSpace(firstName))
            query = query.Where(a => a.FirstName == firstName);
        if (!string.IsNullOrWhiteSpace(lastName))
            query = query.Where(a => a.LastName == lastName);
        if (!string.IsNullOrWhiteSpace(address))
            query = query.Where(a => a.Address == address);
        if (!string.IsNullOrWhiteSpace(postalCode))
            query = query.Where(a => a.PostalCode == postalCode);
        if (!string.IsNullOrWhiteSpace(city))
            query = query.Where(a => a.City == city);
        if (!string.IsNullOrWhiteSpace(phone))
            query = query.Where(a => a.MobilePhone == phone);
        if (!string.IsNullOrWhiteSpace(email))
            query = query.Where(a => a.Email == email);
        if (active)
            query = query.Where(a => a.Active == active);
        if (!string.IsNullOrWhiteSpace(genre))
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
        if (!string.IsNullOrWhiteSpace(nationality))
            query = query.Where(a => a.Nationality == nationality);
        if (!string.IsNullOrWhiteSpace(birthplace))
            query = query.Where(a => a.Birthplace == birthplace);
        if (photo != Guid.Empty)
            query = query.Where(a => a.ProfilePhotoId == photo);
        if (totalWorkHours >= 0)
            query = query.Where(a => a.TotalWorkHours == totalWorkHours);
        if (enrollmentDate != default)
            query = query.Where(a => a.EnrollDate == enrollmentDate);

        var students = query.ToList();
        return students;
    }


    public static List<Student> ConsultStudents(
        string selectedProperty, object selectedValue)
    {
        var property = typeof(Student).GetProperty(selectedProperty);
        if (property == null) return new List<Student>();

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
            return new List<Student>();
        }
        catch (FormatException ex)
        {
            // Handle format exception
            Console.WriteLine($"Invalid format: {ex.Message}");
            return new List<Student>();
        }

        return StudentsList
            .Where(s => property.GetValue(s)?
                .Equals(convertedValue) == true)
            .ToList();
    }


    private static int GetLastIndex()
    {
        //return lastStudent = StudentsList.LastOrDefault();
        return StudentsList.LastOrDefault()?.Id ?? -1;
        //return lastStudent?.Id ?? -1;
    }


    public static int GetLastId()
    {
        var lastStudent = StudentsList.LastOrDefault();
        return lastStudent?.Id ?? GetLastIndex();
        /*
        return lastStudent != null
            ? lastStudent.Id
            : GetLastIndex();
        // handle the case where the collection is empty
        // return StudentsList[^1].Id;
        // return GetLastIndex();
        */
    }


    public static string GetFullName(int id)
    {
        return $"{StudentsList[id].FirstName} {StudentsList[id].LastName}";
    }


    public static string GetFullInfo(int id)
    {
        return $"{StudentsList[id].Id,5} | " +
               //$"{StudentsList[id].GetFullName()} | " +
               $"{GetFullName(id)} | " +
               $"{StudentsList[id].MobilePhone} - {StudentsList[id].Address}";
    }


    public static void CalculateTeacherMetrics()
    {
        if (StudentsList.Count < 1)
            Log.Warning("No teachers found in the directory");

        foreach (var student in StudentsList)
        {
            student.CalculateTotalWorkHours();
            student.CountCourses();
            //teacher.CalculateWorkloadPerCourse();

            Log.Information(
                string.Format(
                    "Metrics for {0}: " +
                    "Total work hours = {1}, " +
                    "Course count = {2}, " +
                    "Workload per course = {3}.",
                    student.FirstName, student.TotalWorkHours,
                    student.CoursesCount));
        }

        Log.Information("Teacher metrics calculation completed");
    }

    #endregion
}