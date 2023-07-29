using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.ExtraEntities;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Seeders;

public class SeedDbPersons
{
    private static Random _random;
    private static IUserHelper _userHelper;
    private static ILogger<SeedDbPersons> _logger;
    private static DataContextMsSql _dataContextMssql;


    // Add a constructor to receive IUserHelper through dependency injection
    public static void Initialize(
        IUserHelper userHelper, ILogger<SeedDbPersons> logger,
        DataContextMsSql dataContextMssql
    )
    {
        _logger = logger;
        _random = new Random();
        _userHelper = userHelper;
        _dataContextMssql = dataContextMssql;
    }


    public static async Task AddingData()
    {
        var user =
            await _userHelper.GetUserByEmailAsync(
                "nuno.santos.26288@formandos.cinel.pt");

        Console.WriteLine(
            "Seeding the users table with students and teachers...");

        if (!_dataContextMssql.Students.Any())
        {
            await AddStudents(user);
        }

        if (!_dataContextMssql.Teachers.Any())
        {
            await AddTeachers(user);
        }
    }


    private static async Task AddStudents(User user)
    {
        var students = new List<Student>();

        var city =
            await _dataContextMssql.Cities
                .FirstOrDefaultAsync(c => c.Name == "Porto");

        var country =
            await _dataContextMssql.Countries
                .FirstOrDefaultAsync(c => c.Name == "Portugal");

        var countryOfNationality =
            await _dataContextMssql.Countries
                .Include(c => c.Nationality.Name == "Francesa")
                .FirstOrDefaultAsync();

        var birthplace =
            await _dataContextMssql.Countries
                .FirstOrDefaultAsync(c => c.Name == "França");


        var genre =
            await _dataContextMssql.Genres
                .FirstOrDefaultAsync(g => g.Name == "Female");

        for (var i = 1; i <= 10; i++)
        {
            var firstName = "Student " + i;
            var lastName = "Lastname " + i;

            // Generate a valid email address based on firstName and lastName
            var email = GenerateValidEmail(firstName, lastName);

            var document =
                _random.Next(100_000_000, 999_999_999).ToString();
            var fixedPhone =
                _random.Next(100_000_000, 999_999_999).ToString();

            var cellPhone =
                _random.Next(100_000_000, 999_999_999).ToString();
            var address = "address " + i + ", " + _random.Next(1, 9_999);
            var idNumber =
                _random.Next(100_000_000, 999_999_999).ToString();
            var vatNumber =
                _random.Next(100_000_000, 999_999_999).ToString();
            var postalCode =
                _random.Next(1_000, 9_999) + "-" + _random.Next(100, 999);
            var dateOfBirth = GenerateRandomDateOfBirth();

            var student = await CreateStudent(
                firstName, lastName, address, email, postalCode,
                cellPhone, dateOfBirth, idNumber, vatNumber,
                user, city, country, countryOfNationality, birthplace, genre);

            students.Add(student);
        }

        await _dataContextMssql.Students.AddRangeAsync(students);

        await _dataContextMssql.SaveChangesAsync();
    }


    private static async Task<Student> CreateStudent(
        string firstName, string lastName, string address, string email,
        string postalCode, string cellPhone, DateTime dateOfBirth,
        string idNumber, string vatNumber,
        User user, City city, Country country,
        Country countryOfNationality, Country birthplace, Genre genre)
    {
        // Create a student object
        var student = new Student
        {
            User = await GetOrCreateUserAsync(
                firstName, lastName,
                email,
                user),
            FirstName = firstName,
            LastName = lastName,
            Address = address,
            PostalCode = postalCode,
            City = city,
            Country = country,
            MobilePhone = cellPhone,
            Email = email,
            Active = true,
            Genre = genre,
            DateOfBirth = dateOfBirth,
            IdentificationNumber = idNumber,
            IdentificationType = "C/C",
            ExpirationDateIdentificationNumber =
                DateTime.Now.ToUniversalTime().AddYears(20),
            TaxIdentificationNumber = vatNumber,
            CountryOfNationality = countryOfNationality,
            Birthplace = birthplace,
            EnrollDate = DateTime.Now.ToUniversalTime(),
            IdGuid = default,
            CreatedBy = user,
        };


        return student;
    }


    private static async Task<User> GetOrCreateUserAsync(
        string firstName, string lastName, string email, User user)
    {
        var existingUser =
            await _dataContextMssql.Users.FirstOrDefaultAsync(u =>
                u.Email == email);

        // User already exists, return the existing user
        if (existingUser != null) return existingUser;

        // User doesn't exist, create a new user
        var newUser = await SeedDbUsers.VerifyUserAsync(
            firstName, lastName,
            email, email,
            "FixedPhone", "Student",
            "Document", "Address");

        // Add the new user to the context
        _dataContextMssql.Users.Add(newUser);

        return newUser;
    }


    private static async Task AddTeachers(User user)
    {
        var teachers = new List<Teacher>();

        var city =
            await _dataContextMssql.Cities
                .FirstOrDefaultAsync(c => c.Name == "Porto");

        var country =
            await _dataContextMssql.Countries
                .FirstOrDefaultAsync(c => c.Name == "Portugal");

        var countryOfNationality =
            await _dataContextMssql.Countries
                .Include(c => c.Nationality.Name == "Francesa")
                .FirstOrDefaultAsync();

        var birthplace =
            await _dataContextMssql.Countries
                .FirstOrDefaultAsync(c => c.Name == "França");


        var genre =
            await _dataContextMssql.Genres
                .FirstOrDefaultAsync(g => g.Name == "Female");

        for (var i = 1; i <= 10; i++)
        {
            var firstName = "Teacher " + i;
            var lastName = "Lastname " + i;

            // Generate a valid email address based on firstName and lastName
            var email = GenerateValidEmail(firstName, lastName);

            var document =
                _random.Next(100_000_000, 999_999_999).ToString();
            var fixedPhone =
                _random.Next(100_000_000, 999_999_999).ToString();

            var cellPhone =
                _random.Next(100_000_000, 999_999_999).ToString();
            var address = "address " + i + ", " + _random.Next(1, 9_999);
            var idNumber =
                _random.Next(100_000_000, 999_999_999).ToString();
            var vatNumber =
                _random.Next(100_000_000, 999_999_999).ToString();
            var postalCode =
                _random.Next(1_000, 9_999) + "-" + _random.Next(100, 999);
            var dateOfBirth = GenerateRandomDateOfBirth();

            var student = await CreateTeacher(
                firstName, lastName, address, email, postalCode,
                cellPhone, dateOfBirth, idNumber, vatNumber,
                user, city, country, countryOfNationality, birthplace, genre);

            teachers.Add(student);
        }

        await _dataContextMssql.Teachers.AddRangeAsync(teachers);

        await _dataContextMssql.SaveChangesAsync();
    }


    private static async Task<Teacher> CreateTeacher(
        string firstName, string lastName, string address, string email,
        string postalCode, string cellPhone, DateTime dateOfBirth,
        string idNumber, string vatNumber,
        User user, City city, Country country,
        Country countryOfNationality, Country birthplace, Genre genre)
    {
        // Create a student object
        var teacher = new Teacher
        {
            User = await GetOrCreateUserAsync(
                firstName, lastName,
                email,
                user),
            FirstName = firstName,
            LastName = lastName,
            Address = address,
            PostalCode = postalCode,
            City = city,
            Country = country,
            MobilePhone = cellPhone,
            Email = email,
            Active = true,
            Genre = genre,
            DateOfBirth = dateOfBirth,
            IdentificationNumber = idNumber,
            IdentificationType = "C/C",
            ExpirationDateIdentificationNumber =
                DateTime.Now.ToUniversalTime().AddYears(20),
            TaxIdentificationNumber = vatNumber,
            CountryOfNationality = countryOfNationality,
            Birthplace = birthplace,
            EnrollDate = DateTime.Now.ToUniversalTime(),
            IdGuid = default,
            CreatedBy = user,
        };


        return teacher;
    }


    private static DateTime GenerateRandomDateOfBirth()
    {
        var random = new Random();

        var startYear = 1950;
        var endYear = 2000;


        var year = random.Next(startYear, endYear + 1);
        var month = random.Next(1, 13);
        var maxDay = DateTime.DaysInMonth(year, month);
        var day = random.Next(1, maxDay + 1);

        Console.WriteLine(new DateTime(year, month, day));
        return new DateTime(year, month, day);
    }


    private static string GenerateValidEmail(string firstName, string lastName)
    {
        //  Define a regular expression for repeated words.
        var pattern = @"[^0-9a-zA-Z]+";

        // Remove any spaces and special characters from the names
        var sanitizedFirstName =
            Regex.Replace(firstName, pattern, "");
        var sanitizedLastName =
            Regex.Replace(lastName, pattern, "");

        // Concatenate the sanitized names to create the email address
        var email = $"{sanitizedFirstName}.{sanitizedLastName}@mail.pt";

        // You can also convert the email to lowercase, if desired
        // email = email.ToLower();

        return email;
    }


    private static async Task StoreStudentsOrTeachersWithRoles<T>(
        EntityEntry<T> studentOrTeacherWithRole, string userRole)
        where T : class
    {
        // Check if the user already exists in the database
        // Check if the user already exists in the database
        var newUser = await _dataContextMssql.Users
            .FirstOrDefaultAsync(u =>
                u.Email == studentOrTeacherWithRole.Entity.GetType()
                    .GetProperty("Email")
                    .GetValue(studentOrTeacherWithRole.Entity, null)
                    .ToString());


        // Check if the role already exists in the database
        var role = await _dataContextMssql.Roles
            .FirstOrDefaultAsync(iR => iR.Name == userRole);

        // If the role does not exist, create it
        if (role == null)
        {
            role = new IdentityRole
            {
                // Set the name for the role
                Name = userRole,
            };

            _dataContextMssql.Roles.Add(role);
        }


        // Assign the role to the user
        // await _userHelper.AddUserAsync(newUser, password);

        // Save the changes to the student entity,
        await _dataContextMssql.SaveChangesAsync();
    }
}