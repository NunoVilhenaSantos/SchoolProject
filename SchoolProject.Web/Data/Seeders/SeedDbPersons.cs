using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.ExtraEntities;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.EntitiesMatrix;
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


        Console.WriteLine(
            "Seeding the users table with students...");

        var studentNames = new List<string>
        {
            "Ana Ribeiro", "Bruno Ferreira", "Catarina Palma",
            "Claudia Passarinho", "Dário Dias", "Diogo Alves", "Diogo Santana",
            "Duarte Marques", "Filipe Baptista", "Joel Santo Rangel",
            "Jorge Pinto", "Láercio Rodrigues", "Licínio Rosário",
            "Luis Leopoldo", "Luis Patricio", "Maria Cristina",
            "Mariana Oliveira", "Mariana Leite", "Nuno Vilhena Santos",
            "Pedro Silva", "Reinaldo Souza", "Ruben Correia", "Simão André",
            "Tatiane Avellar", "Tiago Silva", "Vasco Santos", "Vitor Santos",
            "Vitor Silva", "Vitorino Freitas", "Vitorino Silva",
            "New Student 1", "New Student 2", "New Student 3", "New Student 4",
            "New Student 5", "New Student 6", "New Student 7", "New Student 8",
            "New Student 9", "New Student 10", "New Student 11",
            "New Student 12", "New Student 13", "New Student 14",
            "New Student 15", "New Student 16", "New Student 17",
            "New Student 18", "New Student 19", "New Student 20"
        };


        // Check if any students need to be added
        var existingStudents =
            await _dataContextMssql.Students.ToListAsync();
        var studentsToAdd = studentNames
            .Except(
                existingStudents
                    .Select(s => s.FirstName + " " + s.LastName))
            .ToList();

        if (studentsToAdd.Any()) await AddStudents(user, studentsToAdd);


        Console.WriteLine(
            "Seeding the users table with teachers...");

        var teacherNames = new List<string>
        {
            "New Teacher 1", "New Teacher 2", "New Teacher 3", "New Teacher 4",
            "New Teacher 5", "New Teacher 6", "New Teacher 7", "New Teacher 8",
            "New Teacher 9", "New Teacher 10", "New Teacher 11",
            "New Teacher 12", "New Teacher 13", "New Teacher 14",
            "New Teacher 15", "New Teacher 16", "New Teacher 17",
            "New Teacher 18", "New Teacher 19", "New Teacher 20",
            "New Teacher 21", "New Teacher 22", "New Teacher 23",
            "New Teacher 12", "New Teacher 13", "New Teacher 14",
            "New Teacher 15", "New Teacher 16", "New Teacher 17",
            "New Teacher 18", "New Teacher 19", "New Teacher 20",
            "New Teacher 21", "New Teacher 22", "New Teacher 23",
            "New Teacher 24", "New Teacher 25", "New Teacher 26",
            "New Teacher 27", "New Teacher 28", "New Teacher 29",
            "New Teacher 30", "New Teacher 31", "New Teacher 32",
            "New Teacher 33", "New Teacher 34", "New Teacher 35",
            "New Teacher 36", "New Teacher 37", "New Teacher 38",
            "New Teacher 39", "New Teacher 40", "New Teacher 41",
            "New Teacher 42", "New Teacher 43", "New Teacher 44",
            "New Teacher 45", "New Teacher 46", "New Teacher 47",
            "New Teacher 48", "New Teacher 49", "New Teacher 50",
            "New Teacher 51", "New Teacher 52", "New Teacher 53",
            "New Teacher 54", "New Teacher 55", "New Teacher 56",
            "New Teacher 57", "New Teacher 58", "New Teacher 59"
        };

        // Check if any teachers need to be added
        var existingTeachers =
            await _dataContextMssql.Teachers.ToListAsync();
        var teachersToAdd = teacherNames
            .Except(
                existingTeachers
                    .Select(t => t.FirstName + " " + t.LastName))
            .ToList();

        if (teachersToAdd.Any()) await AddTeachers(user, teachersToAdd);
    }


    private static async Task AddStudents(
        User user, List<string> studentNamesToAdd)
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
                // Include the Nationality related entity
                .Include(c => c.Nationality)
                .FirstOrDefaultAsync(c => c.Nationality.Name == "French");

        var birthplace =
            await _dataContextMssql.Countries
                .FirstOrDefaultAsync(c => c.Name == "France");

        var genre =
            await _dataContextMssql.Genres
                .FirstOrDefaultAsync(g => g.Name == "Female");


        foreach (var studentName in studentNamesToAdd)
        {
            var firstName = studentName.Split(' ')[0];
            var lastName = studentName.Split(' ')[1];

            // Generate a valid email address based on firstName and lastName
            // var firstName = "Student " + i;
            // var lastName = "Lastname " + i;

            // Generate a valid email address based on firstName and lastName
            var email = GenerateValidEmail(firstName, lastName);

            var document =
                _random.Next(100_000_000, 999_999_999).ToString();
            var fixedPhone =
                _random.Next(100_000_000, 999_999_999).ToString();
            var cellPhone =
                _random.Next(100_000_000, 999_999_999).ToString();
            var address =
                "Address of " + firstName + " " + lastName + ", " +
                _random.Next(1, 9_999);
            var idNumber =
                _random.Next(100_000_000, 999_999_999).ToString();
            var vatNumber =
                _random.Next(100_000_000, 999_999_999).ToString();
            var postalCode =
                _random.Next(1_000, 9_999) + "-" + _random.Next(100, 999);
            var dateOfBirth = GenerateRandomDateOfBirth();

            Console.WriteLine(
                "Creating student: " + firstName + " " + lastName);

            var student = CreateStudent(
                firstName, lastName, address, email, postalCode,
                cellPhone, dateOfBirth, idNumber, vatNumber,
                user, city, country, countryOfNationality, birthplace, genre);

            students.Add(student);
        }

        await _dataContextMssql.Students.AddRangeAsync(students);

        await _dataContextMssql.SaveChangesAsync();
    }


    private static Student CreateStudent(
        string firstName, string lastName, string address, string email,
        string postalCode, string cellPhone, DateTime dateOfBirth,
        string idNumber, string vatNumber,
        User user, City city, Country country,
        Country countryOfNationality, Country birthplace, Genre genre)
    {
        // var newUser = await GetOrCreateUserAsync(
        //     firstName, lastName,
        //     email, cellPhone,
        //     "document " + email,
        //     address,
        //     user);

        var newUser = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Address = address,
            UserName = email,
            Email = email,
            PhoneNumber = cellPhone,
            WasDeleted = false
        };

        Console.WriteLine("Creating student: " + firstName + " " + lastName);

        // Create a student object
        var student = new Student
        {
            User = newUser,
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
            CreatedBy = user
        };


        return student;
    }


    private static async Task<User> GetOrCreateUserAsync(string firstName,
        string lastName, string email, string mobilePhone,
        string document, string address, User user,
        string role = "Student")
    {
        var existingUser =
            await _dataContextMssql.Users.FirstOrDefaultAsync(u =>
                u.Email == email);

        // User already exists, return the existing user
        if (existingUser != null) return existingUser;

        // User doesn't exist, create a new user
        var newUser = await SeedDbUsers.VerifyUserAsync(
            firstName, lastName,
            email,
            email,
            mobilePhone, role,
            document, address);

        return newUser;
    }


    private static async Task AddTeachers(
        User user, List<string> teacherNamesToAdd)
    {
        var teachers = new List<Teacher>();

        var city =
            await _dataContextMssql.Cities
                .FirstOrDefaultAsync(c => c.Name == "Faro");

        var country =
            await _dataContextMssql.Countries
                .FirstOrDefaultAsync(c => c.Name == "Portugal");

        var countryOfNationality =
            await _dataContextMssql.Countries
                // Include the Nationality related entity
                .Include(c => c.Nationality)
                .FirstOrDefaultAsync(c => c.Nationality.Name == "French");

        var birthplace =
            await _dataContextMssql.Countries
                .FirstOrDefaultAsync(c => c.Name == "Angola");

        var genre =
            await _dataContextMssql.Genres
                .FirstOrDefaultAsync(g => g.Name == "Female");

        foreach (var teacherName in teacherNamesToAdd)
        {
            var firstName = teacherName.Split(' ')[0];
            var lastName = teacherName.Split(' ')[1];

            // var firstName = "Teacher " + i;
            // var lastName = "Lastname " + i;

            // Generate a valid email address based on firstName and lastName
            var email = GenerateValidEmail(firstName, lastName);

            var document =
                _random.Next(100_000_000, 999_999_999).ToString();
            var fixedPhone =
                _random.Next(100_000_000, 999_999_999).ToString();
            var cellPhone =
                _random.Next(100_000_000, 999_999_999).ToString();
            var address =
                "Address of " + firstName + " " + lastName + ", " +
                _random.Next(1, 9_999);
            var idNumber =
                _random.Next(100_000_000, 999_999_999).ToString();
            var vatNumber =
                _random.Next(100_000_000, 999_999_999).ToString();
            var postalCode =
                _random.Next(1_000, 9_999) + "-" + _random.Next(100, 999);
            var dateOfBirth = GenerateRandomDateOfBirth();

            Console.WriteLine("Teacher: " + firstName + " " + lastName);

            var student = CreateTeacher(
                firstName, lastName, address, email, postalCode,
                cellPhone, dateOfBirth, idNumber, vatNumber,
                user, city, country, countryOfNationality, birthplace, genre);

            teachers.Add(student);
        }

        await _dataContextMssql.Teachers.AddRangeAsync(teachers);

        await _dataContextMssql.SaveChangesAsync();
    }


    private static Teacher CreateTeacher(
        string firstName, string lastName, string address, string email,
        string postalCode, string cellPhone, DateTime dateOfBirth,
        string idNumber, string vatNumber,
        User user, City city, Country country,
        Country countryOfNationality, Country birthplace, Genre genre)
    {
        // var newUser = await GetOrCreateUserAsync(
        //     firstName, lastName,
        //     email, cellPhone,
        //     "document " + email,
        //     address,
        //     user);

        var newUser = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Address = address,
            UserName = email,
            Email = email,
            PhoneNumber = cellPhone,
            WasDeleted = false
        };


        Console.WriteLine("Creating teacher: " + firstName + " " + lastName);

        // Create a teacher object
        var teacher = new Teacher
        {
            User = newUser,
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
            CreatedBy = user
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

        Console.WriteLine("Date of birth generated: " +
                          new DateTime(year, month, day));
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
                Name = userRole
            };

            _dataContextMssql.Roles.Add(role);
        }


        // Assign the role to the user
        // await _userHelper.AddUserAsync(newUser, password);

        // Save the changes to the student entity,
        await _dataContextMssql.SaveChangesAsync();
    }
}