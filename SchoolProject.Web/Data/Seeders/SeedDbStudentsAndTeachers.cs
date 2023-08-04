using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.ExtraEntities;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.EntitiesOthers;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Seeders;

public class SeedDbStudentsAndTeachers
{
    private static Random _random;
    private static IUserHelper _userHelper;
    private static ILogger<SeedDbStudentsAndTeachers> _logger;
    private static DataContextMsSql _dataContextMsSql;


    private static List<User> _listOfUsersFromDb;
    private static List<Student> _listOfStudentsFromDb;
    private static List<Teacher> _listOfTeachersFromDb;


    private static readonly List<User> _listOfUsersToAdd = new List<User>();

    private static readonly List<Student> _listOfStudentsToAdd =
        new List<Student>();

    private static readonly List<Teacher> _listOfTeachersToAdd =
        new List<Teacher>();


    public SeedDbStudentsAndTeachers(
        // User user,
        IUserHelper userHelper,
        DataContextMsSql dataContextMsSql,
        ILogger<SeedDbStudentsAndTeachers> logger
    )
    {
        // _user = user;
        _logger = logger;
        _random = new Random();
        _userHelper = userHelper;
        _dataContextMsSql = dataContextMsSql;
    }


    // Add a constructor to receive IUserHelper through dependency injection
    public static void Initialize(
        IUserHelper userHelper,
        DataContextMsSql dataContextMssql,
        ILogger<SeedDbStudentsAndTeachers> logger
    )
    {
        _logger = logger;
        _random = new Random();
        _userHelper = userHelper;
        _dataContextMsSql = dataContextMssql;
    }


    public static async Task AddingData(User user)
    {
        Console.WriteLine(
            "Seeding the users table with students and teachers...");


        // ------------------------------------------------------------------ //
        var existingUsersList =
            await _dataContextMsSql.Users.ToListAsync();
        _listOfUsersFromDb = existingUsersList.ToList();


        // ------------------------------------------------------------------ //
        var existingStudents =
            await _dataContextMsSql.Students.ToListAsync();
        _listOfStudentsFromDb = existingStudents.ToList();


        // ------------------------------------------------------------------ //
        var existingTeachers =
            await _dataContextMsSql.Teachers.ToListAsync();
        _listOfTeachersFromDb = existingTeachers.ToList();


        // ------------------------------------------------------------------ //
        Console.WriteLine(
            "Seeding the users table with students...");
        await GenerateStudentsNames(user, _listOfStudentsFromDb);

        Console.WriteLine(
            "Seeding the users table with teachers...");
        await GenerateTeachersNames(user, _listOfTeachersFromDb);
    }


    private static async Task GenerateStudentsNames(
        User user, IEnumerable<Student> existingStudentsList)
    {
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


        var studentsToAdd = studentNames
            .Except(
                existingStudentsList
                    .Select(s => s.FirstName + " " + s.LastName))
            .ToList();

        if (studentsToAdd.Any())
            await AddStudentsOrTeachers(user, studentsToAdd, "Student");
    }


    private static async Task GenerateTeachersNames(
        User user, IEnumerable<Teacher> existingTeachersList)
    {
        var teacherNames = new List<string>
        {
            "New Teacher 1", "New Teacher 2", "New Teacher 3", "New Teacher 4",
            "New Teacher 5", "New Teacher 6", "New Teacher 7", "New Teacher 8",
            "New Teacher 9", "New Teacher 10", "New Teacher 11",
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

        var teachersToAdd = teacherNames
            .Except(
                existingTeachersList
                    .Select(t => t.FirstName + " " + t.LastName))
            .ToList();

        if (teachersToAdd.Any())
            await AddStudentsOrTeachers(user, teachersToAdd, "Teacher");
    }


    private static async Task AddStudentsOrTeachers(
        // Novo parâmetro para indicar o papel (role)
        User user, List<string> namesToAdd, string userRole)
    {
        var city =
            await _dataContextMsSql.Cities
                .FirstOrDefaultAsync(c => c.Name == "Porto");
        var country =
            await _dataContextMsSql.Countries
                .FirstOrDefaultAsync(c => c.Name == "Portugal");
        var countryOfNationality =
            await _dataContextMsSql.Countries
                .Include(c => c.Nationality)
                .FirstOrDefaultAsync(c => c.Nationality.Name == "French");
        var birthplace =
            await _dataContextMsSql.Countries
                .FirstOrDefaultAsync(c => c.Name == "France");
        var genre =
            await _dataContextMsSql.Genres
                .FirstOrDefaultAsync(g => g.Name == "Female");


        foreach (var name in namesToAdd)
        {
            var firstName = name.Split(' ')[0];
            var lastName = name.Replace(firstName, "").Trim();

            // Generate a valid email address based on firstName and lastName
            var email = GenerateValidEmail(firstName, lastName);

            var document = _random.Next(100_000_000, 999_999_999).ToString();
            var fixedPhone = _random.Next(100_000_000, 999_999_999).ToString();
            var cellPhone = _random.Next(100_000_000, 999_999_999).ToString();
            var address =
                "Address of " + firstName + " " + lastName + ", " +
                _random.Next(1, 9_999);
            var idNumber = _random.Next(100_000_000, 999_999_999).ToString();
            var vatNumber = _random.Next(100_000_000, 999_999_999).ToString();
            var postalCode =
                _random.Next(1_000, 9_999) + "-" + _random.Next(100, 999);
            var dateOfBirth = GenerateRandomDateOfBirth();


            Console.WriteLine(
                $"Creating {userRole.ToLower()}: {firstName} {lastName}");


            if (userRole.Equals(
                    "Student", StringComparison.OrdinalIgnoreCase))
            {
                // var student = await CreateStudent(
                //     firstName, lastName, address, email, postalCode,
                //     cellPhone, dateOfBirth, idNumber, vatNumber,
                //     user, city, country, countryOfNationality, birthplace,
                //     genre);

                var student = await CreateUser<Student>(
                    firstName, lastName, address, email, postalCode,
                    cellPhone, dateOfBirth, idNumber, vatNumber,
                    user, city, country, countryOfNationality, birthplace,
                    genre, "Student");

                _listOfStudentsToAdd.Add(student);
            }
            else if (userRole.Equals(
                         "Teacher", StringComparison.OrdinalIgnoreCase))
            {
                // var teacher = await CreateTeacher(
                //     firstName, lastName, address, email, postalCode,
                //     cellPhone, dateOfBirth, idNumber, vatNumber,
                //     user, city, country, countryOfNationality, birthplace,
                //     genre);

                var teacher = await CreateUser<Teacher>(
                    firstName, lastName, address, email, postalCode,
                    cellPhone, dateOfBirth, idNumber, vatNumber,
                    user, city, country, countryOfNationality, birthplace,
                    genre, "Teacher");

                _listOfTeachersToAdd.Add(teacher);
            }
            else
            {
                try
                {
                    throw new Exception(
                        $"Invalid userRole: {userRole}");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }
        

        Console.WriteLine($"Created {namesToAdd.Count} {userRole.ToLower()}s");

        
        // ------------------------------------------------------------------ //
        Console.WriteLine($"Saving {userRole.ToLower()}s to the database...");
        await StoreStudentsOrTeachersWithRoles(
            user, _listOfStudentsToAdd, "Student");
        await StoreStudentsOrTeachersWithRoles(
            user, _listOfTeachersToAdd, "Teacher");


        // ------------------------------------------------------------------ //
        var existingUsersList =
            await _dataContextMsSql.Users.ToListAsync();
        var _existingUsersList = existingUsersList.ToList();


        // ------------------------------------------------------------------ //
        // TODO: add students or teachers to the database
        if (userRole.Equals(
                "Student", StringComparison.OrdinalIgnoreCase))
        {
            await _dataContextMsSql.Students
                .AddRangeAsync(_listOfStudentsToAdd);
        }
        else if (userRole.Equals(
                     "Teacher", StringComparison.OrdinalIgnoreCase))
        {
            await _dataContextMsSql.Teachers
                .AddRangeAsync(_listOfTeachersToAdd);
        }

        await _dataContextMsSql.SaveChangesAsync();
    }


    private static async Task<T> CreateUser<T>(
        string firstName, string lastName, string address, string email,
        string postalCode, string cellPhone, DateTime dateOfBirth,
        string idNumber, string vatNumber,
        User user, City city, Country country,
        Country countryOfNationality, Country birthplace, Genre genre,
        // Novo parâmetro para indicar o papel (role)
        string userRole, string password = "Passw0rd"
        // Restrição genérica para permitir apenas classes
    ) where T : class
    {
        var newUser = new SchoolProject.Web.Data.EntitiesOthers.User
        {
            FirstName = firstName,
            LastName = lastName,
            Address = address,
            UserName = email,
            Email = email,
            PhoneNumber = cellPhone,
            WasDeleted = false
        };

        _listOfUsersToAdd.Add(newUser);

        // await _userHelper.AddUserAsync(newUser, password);
        // await _userHelper.AddUserToRoleAsync(newUser, userRole);
        // newUser = await _userHelper.GetUserByEmailAsync(newUser.Email);


        Console.WriteLine(
            $"Creating {userRole.ToLower()}: {firstName} {lastName}");


        // Create the specific entity (Student or Teacher) based on the generic type T
        var entity = Activator.CreateInstance<T>();
        entity.GetType().GetProperty("User")?.SetValue(entity, newUser);
        entity.GetType().GetProperty("FirstName")?.SetValue(entity, firstName);
        entity.GetType().GetProperty("LastName")?.SetValue(entity, lastName);
        entity.GetType().GetProperty("Address")?.SetValue(entity, address);
        entity.GetType().GetProperty("PostalCode")
            ?.SetValue(entity, postalCode);
        entity.GetType().GetProperty("City")?.SetValue(entity, city);
        entity.GetType().GetProperty("Country")?.SetValue(entity, country);
        entity.GetType().GetProperty("MobilePhone")
            ?.SetValue(entity, cellPhone);
        entity.GetType().GetProperty("Email")?.SetValue(entity, email);
        entity.GetType().GetProperty("Active")?.SetValue(entity, true);
        entity.GetType().GetProperty("Genre")?.SetValue(entity, genre);
        entity.GetType().GetProperty("DateOfBirth")
            ?.SetValue(entity, dateOfBirth);
        entity.GetType().GetProperty("IdentificationNumber")
            ?.SetValue(entity, idNumber);
        entity.GetType().GetProperty("IdentificationType")
            ?.SetValue(entity, "C/C");
        entity.GetType().GetProperty("ExpirationDateIdentificationNumber")
            ?.SetValue(entity, DateTime.Now.ToUniversalTime().AddYears(20));
        entity.GetType().GetProperty("TaxIdentificationNumber")
            ?.SetValue(entity, vatNumber);
        entity.GetType().GetProperty("CountryOfNationality")
            ?.SetValue(entity, countryOfNationality);
        entity.GetType().GetProperty("Birthplace")
            ?.SetValue(entity, birthplace);
        entity.GetType().GetProperty("EnrollDate")
            ?.SetValue(entity, DateTime.Now.ToUniversalTime());
        entity.GetType().GetProperty("IdGuid")?.SetValue(entity, default);
        entity.GetType().GetProperty("CreatedBy")?.SetValue(entity, user);

        return entity;
    }


    // private static async Task AddStudentsOrTeachers(
    //     EntitiesMatrix.User user, List<string> studentNamesToAdd)
    // {
    //     var city =
    //         await _dataContextMsSql.Cities
    //             .FirstOrDefaultAsync(c => c.Name == "Porto");
    //
    //     var country =
    //         await _dataContextMsSql.Countries
    //             .FirstOrDefaultAsync(c => c.Name == "Portugal");
    //
    //     var countryOfNationality =
    //         await _dataContextMsSql.Countries
    //             // Include the Nationality related entity
    //             .Include(c => c.Nationality)
    //             .FirstOrDefaultAsync(c => c.Nationality.Name == "French");
    //
    //     var birthplace =
    //         await _dataContextMsSql.Countries
    //             .FirstOrDefaultAsync(c => c.Name == "France");
    //
    //     var genre =
    //         await _dataContextMsSql.Genres
    //             .FirstOrDefaultAsync(g => g.Name == "Female");
    //
    //
    //     foreach (var studentName in studentNamesToAdd)
    //     {
    //         var firstName = studentName.Split(' ')[0];
    //         var lastName =
    //             studentName.Replace(firstName, "").Trim();
    //
    //
    //         // Generate a valid email address based on firstName and lastName
    //         var email = GenerateValidEmail(firstName, lastName);
    //
    //         var document =
    //             _random.Next(100_000_000, 999_999_999).ToString();
    //         var fixedPhone =
    //             _random.Next(100_000_000, 999_999_999).ToString();
    //         var cellPhone =
    //             _random.Next(100_000_000, 999_999_999).ToString();
    //         var address =
    //             "Address of " + firstName + " " + lastName + ", " +
    //             _random.Next(1, 9_999);
    //         var idNumber =
    //             _random.Next(100_000_000, 999_999_999).ToString();
    //         var vatNumber =
    //             _random.Next(100_000_000, 999_999_999).ToString();
    //         var postalCode =
    //             _random.Next(1_000, 9_999) + "-" + _random.Next(100, 999);
    //         var dateOfBirth = GenerateRandomDateOfBirth();
    //
    //         Console.WriteLine(
    //             "Creating student: " + firstName + " " + lastName);
    //
    //         var student = CreateStudent(
    //             firstName, lastName, address, email, postalCode,
    //             cellPhone, dateOfBirth, idNumber, vatNumber,
    //             user, city, country, countryOfNationality, birthplace, genre);
    //
    //         // students.Add(student);
    //     }
    //
    //     Console.WriteLine("Saving students to the database...");
    //
    //     // todo: add students to the database
    //     // await _dataContextMsSql.Students.AddRangeAsync(students);
    //
    //     await _dataContextMsSql.SaveChangesAsync();
    // }


    // private static async Task<Student> CreateStudent(
    //     string firstName, string lastName, string address, string email,
    //     string postalCode, string cellPhone, DateTime dateOfBirth,
    //     string idNumber, string vatNumber,
    //     EntitiesMatrix.User user, City city, Country country,
    //     Country countryOfNationality, Country birthplace, Genre genre,
    //     string password = "Passw0rd")
    // {
    //     var newUser = new EntitiesMatrix.User
    //     {
    //         FirstName = firstName,
    //         LastName = lastName,
    //         Address = address,
    //         UserName = email,
    //         Email = email,
    //         PhoneNumber = cellPhone,
    //         WasDeleted = false
    //     };
    //
    //
    //     //newUser = await SeedDbUsers.VerifyUserAsync(
    //     //    firstName, lastName,
    //     //    email,
    //     //    email,
    //     //    cellPhone, "Student",
    //     //    "document of " + firstName + " " + lastName + " from " + address,
    //     //    address, password
    //     //);
    //
    //     Console.WriteLine("Creating student: " + firstName + " " + lastName);
    //
    //     await _userHelper.AddUserAsync(newUser, password);
    //     await _userHelper.AddUserToRoleAsync(newUser, "Student");
    //     newUser = await _userHelper.GetUserByEmailAsync(newUser.Email);
    //
    //
    //     Console.WriteLine("Debug zone...");
    //
    //
    //     // Create a student object
    //     var student = new Student
    //     {
    //         User = newUser,
    //         FirstName = firstName,
    //         LastName = lastName,
    //         Address = address,
    //         PostalCode = postalCode,
    //         City = city,
    //         Country = country,
    //         MobilePhone = cellPhone,
    //         Email = email,
    //         Active = true,
    //         Genre = genre,
    //         DateOfBirth = dateOfBirth,
    //         IdentificationNumber = idNumber,
    //         IdentificationType = "C/C",
    //         ExpirationDateIdentificationNumber =
    //             DateTime.Now.ToUniversalTime().AddYears(20),
    //         TaxIdentificationNumber = vatNumber,
    //         CountryOfNationality = countryOfNationality,
    //         Birthplace = birthplace,
    //         EnrollDate = DateTime.Now.ToUniversalTime(),
    //         IdGuid = default,
    //         CreatedBy = user
    //     };
    //
    //
    //     return student;
    // }


    // private static async Task<Teacher> CreateTeacher(
    //     string firstName, string lastName, string address, string email,
    //     string postalCode, string cellPhone, DateTime dateOfBirth,
    //     string idNumber, string vatNumber,
    //     EntitiesMatrix.User user, City city, Country country,
    //     Country countryOfNationality, Country birthplace, Genre genre,
    //     string password = "Passw0rd")
    // {
    //     var newUser = new EntitiesMatrix.User
    //     {
    //         FirstName = firstName,
    //         LastName = lastName,
    //         Address = address,
    //         UserName = email,
    //         Email = email,
    //         PhoneNumber = cellPhone,
    //         WasDeleted = false
    //     };
    //
    //
    //     newUser = await SeedDbUsers.VerifyUserAsync(
    //         firstName, lastName,
    //         email,
    //         email,
    //         cellPhone, "Student",
    //         "document of " + firstName + " " + lastName + " from " + address,
    //         address, password
    //     );
    //
    //
    //     Console.WriteLine("Creating teacher: " + firstName + " " + lastName);
    //
    //     // Create a teacher object
    //     var teacher = new Teacher
    //     {
    //         User = newUser,
    //         FirstName = firstName,
    //         LastName = lastName,
    //         Address = address,
    //         PostalCode = postalCode,
    //         City = city,
    //         Country = country,
    //         MobilePhone = cellPhone,
    //         Email = email,
    //         Active = true,
    //         Genre = genre,
    //         DateOfBirth = dateOfBirth,
    //         IdentificationNumber = idNumber,
    //         IdentificationType = "C/C",
    //         ExpirationDateIdentificationNumber =
    //             DateTime.Now.ToUniversalTime().AddYears(20),
    //         TaxIdentificationNumber = vatNumber,
    //         CountryOfNationality = countryOfNationality,
    //         Birthplace = birthplace,
    //         EnrollDate = DateTime.Now.ToUniversalTime(),
    //         IdGuid = default,
    //         CreatedBy = user
    //     };
    //
    //
    //     return teacher;
    // }


    // private static async Task AddTeachers(
    //     EntitiesMatrix.User user, List<string> teacherNamesToAdd)
    // {
    //     var teachers = new List<Teacher>();
    //
    //     var city =
    //         await _dataContextMsSql.Cities
    //             .FirstOrDefaultAsync(c => c.Name == "Faro");
    //
    //     var country =
    //         await _dataContextMsSql.Countries
    //             .FirstOrDefaultAsync(c => c.Name == "Portugal");
    //
    //     var countryOfNationality =
    //         await _dataContextMsSql.Countries
    //             // Include the Nationality related entity
    //             .Include(c => c.Nationality)
    //             .FirstOrDefaultAsync(c => c.Nationality.Name == "French");
    //
    //     var birthplace =
    //         await _dataContextMsSql.Countries
    //             .FirstOrDefaultAsync(c => c.Name == "Angola");
    //
    //     var genre =
    //         await _dataContextMsSql.Genres
    //             .FirstOrDefaultAsync(g => g.Name == "Female");
    //
    //     foreach (var teacherName in teacherNamesToAdd)
    //     {
    //         var firstName = teacherName.Split(' ')[0];
    //         var lastName =
    //             teacherName.Replace(firstName, "").Trim();
    //
    //         // var firstName = "Teacher " + i;
    //         // var lastName = "Lastname " + i;
    //
    //         // Generate a valid email address based on firstName and lastName
    //         var email = GenerateValidEmail(firstName, lastName);
    //
    //         var document =
    //             _random.Next(100_000_000, 999_999_999).ToString();
    //         var fixedPhone =
    //             _random.Next(100_000_000, 999_999_999).ToString();
    //         var cellPhone =
    //             _random.Next(100_000_000, 999_999_999).ToString();
    //         var address =
    //             "Address of " + firstName + " " + lastName + ", " +
    //             _random.Next(1, 9_999);
    //         var idNumber =
    //             _random.Next(100_000_000, 999_999_999).ToString();
    //         var vatNumber =
    //             _random.Next(100_000_000, 999_999_999).ToString();
    //         var postalCode =
    //             _random.Next(1_000, 9_999) + "-" + _random.Next(100, 999);
    //         var dateOfBirth = GenerateRandomDateOfBirth();
    //
    //         Console.WriteLine("Teacher: " + firstName + " " + lastName);
    //
    //         var student = CreateTeacher(
    //             firstName, lastName, address, email, postalCode,
    //             cellPhone, dateOfBirth, idNumber, vatNumber,
    //             user, city, country, countryOfNationality, birthplace, genre);
    //
    //         // teachers.Add(student);
    //     }
    //
    //     // todo: add teachers to the database
    //     // await _dataContextMsSql.Teachers.AddRangeAsync(teachers);
    //
    //     await _dataContextMsSql.SaveChangesAsync();
    // }


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
        email = email.ToLower();

        return email;
    }


    private static async Task StoreStudentsOrTeachersWithRoles<T>(
        User user, List<T> usersToAddList, string userRole,
        string password = "Passw0rd") where T : class
    {
        // Check if the role already exists in the database
        await _userHelper.CheckRoleAsync(userRole);


        foreach (var studentOrTeacherWithRole in usersToAddList)
        {
            var email = studentOrTeacherWithRole
                .GetType().GetProperty("Email")?
                .GetValue(studentOrTeacherWithRole, null).ToString();

            // Get the user from users to add list
            var userFromUsersToAddList =
                _listOfUsersToAdd.FirstOrDefault(u => u.Email == email);


            // Check if the user already exists in the existing users list
            var userExist =
                _listOfUsersFromDb.FirstOrDefault(u => u.Email == email);


            if (userExist != null) continue;


            // If the user doesn't exist in the existing list,
            // create and add the user to the database
            var result =
                await _userHelper.AddUserAsync(
                    userFromUsersToAddList, password);


            if (result.Succeeded)
                await _userHelper
                    .AddUserToRoleAsync(userFromUsersToAddList, userRole);


            // Clear the users to add list
            _listOfUsersToAdd.Remove(userFromUsersToAddList);
        }


        // Save the changes to the student or teacher entity
        await _dataContextMsSql.SaveChangesAsync();
    }
}