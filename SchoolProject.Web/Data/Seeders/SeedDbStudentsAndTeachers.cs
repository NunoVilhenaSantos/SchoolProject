using System.Text.RegularExpressions;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.OtherEntities;
using SchoolProject.Web.Data.Entities.Students;
using SchoolProject.Web.Data.Entities.Teachers;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Seeders;

/// <summary>
///     Seed the database with students and teachers
/// </summary>
public class SeedDbStudentsAndTeachers
{
    private static Random _random;
    private static IUserHelper _userHelper;

    private static DataContextMySql _dataContextInUse;
    // private static DataContextMsSql _dataContextInUse;


    private static readonly HashSet<string>
        ExistingEmailsOfUsersFromDb = new();

    private static List<User>? _listOfUsersFromDb;
    private static List<Student>? _listOfStudentsFromDb;
    private static List<Teacher>? _listOfTeachersFromDb;


    private static readonly List<User> ListOfUsersToAdd = new();
    private static readonly List<Student> ListOfStudentsToAdd = new();
    private static readonly List<Teacher> ListOfTeachersToAdd = new();


    // Add a constructor to receive IUserHelper through dependency injection
    /// <summary>
    ///     Add a constructor to receive IUserHelper through dependency injection
    /// </summary>
    /// <param name="userHelper"></param>
    /// <param name="dataContext"></param>
    public static void Initialize(
        IUserHelper userHelper, DataContextMySql dataContext
    )
    {
        _random = new Random();
        _userHelper = userHelper;
        _dataContextInUse = dataContext;
    }


    public static async Task AddingData(User user)
    {
        Console.WriteLine(
            "Seeding the users table with students and teachers...");

        await PopulateExistingUsersStudentsAndTeachersFromDb();


        // ------------------------------------------------------------------ //
        Console.WriteLine(
            "Seeding the users table with students...");
        await GenerateStudentsNames(user);


        // ------------------------------------------------------------------ //
        Console.WriteLine(
            "Seeding the users table with teachers...");
        await GenerateTeachersNames(user);


        // ------------------------------------------------------------------ //
        Console.WriteLine(
            "Saving Students and Teachers to there respective tables...");

        await PopulateExistingUsersStudentsAndTeachersFromDb();

        await StoreStudentsOrTeachersInDb();
    }


    private static async Task PopulateExistingUsersStudentsAndTeachersFromDb()
    {
        Console.WriteLine("debug zone...");


        // ------------------------------------------------------------------ //
        var existingUsersList =
            await _dataContextInUse.Users.ToListAsync();
        _listOfUsersFromDb = existingUsersList.ToList();


        // Fill the existing emails HashSet for efficient email lookups
        ExistingEmailsOfUsersFromDb
            .UnionWith(_listOfUsersFromDb.Select(u => u.Email));


        // ------------------------------------------------------------------ //
        var existingStudents =
            await _dataContextInUse.Students.ToListAsync();
        _listOfStudentsFromDb = existingStudents.ToList();


        // ------------------------------------------------------------------ //
        var existingTeachers =
            await _dataContextInUse.Teachers.ToListAsync();
        _listOfTeachersFromDb = existingTeachers.ToList();


        Console.WriteLine("debug zone...");
    }


    private static async Task GenerateStudentsNames(User user)
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
            "Student 1", "Student 2", "Student 3", "Student 4",
            "Student 5", "Student 6", "Student 7", "Student 8",
            "Student 9", "Student 10", "Student 11",
            "Student 12", "Student 13", "Student 14",
            "Student 15", "Student 16", "Student 17",
            "Student 18", "Student 19", "Student 20"
        };


        var studentsToAdd = studentNames
            .Except(
                _listOfStudentsFromDb
                    .Select(s => s.FirstName + " " + s.LastName))
            .ToList();

        if (studentsToAdd.Any())
            await AddStudentsOrTeachers(user, studentsToAdd, "Student");

        // await PopulateExistingUsersStudentsAndTeachersFromDb();
        // await StoreStudentsOrTeachersInDb();
    }


    private static async Task GenerateTeachersNames(User user)
    {
        var teacherNames = new List<string>
        {
            "Teacher 1", "Teacher 2", "Teacher 3", "Teacher 4",
            "Teacher 5", "Teacher 6", "Teacher 7", "Teacher 8",
            "Teacher 9", "Teacher 10", "Teacher 11",
            "Teacher 12", "Teacher 13", "Teacher 14",
            "Teacher 15", "Teacher 16", "Teacher 17",
            "Teacher 18", "Teacher 19", "Teacher 20",
            "Teacher 21", "Teacher 22", "Teacher 23",
            "Teacher 24", "Teacher 25", "Teacher 26",
            "Teacher 27", "Teacher 28", "Teacher 29",
            "Teacher 30", "Teacher 31", "Teacher 32",
            "Teacher 33", "Teacher 34", "Teacher 35",
            "Teacher 36", "Teacher 37", "Teacher 38",
            "Teacher 39", "Teacher 40", "Teacher 41",
            "Teacher 42", "Teacher 43", "Teacher 44",
            "Teacher 45", "Teacher 46", "Teacher 47",
            "Teacher 48", "Teacher 49", "Teacher 50",
        };

        var teachersToAdd = teacherNames
            .Except(
                _listOfTeachersFromDb
                    .Select(t => t.FirstName + " " + t.LastName))
            .ToList();

        if (teachersToAdd.Any())
            await AddStudentsOrTeachers(user, teachersToAdd, "Teacher");

        // await PopulateExistingUsersStudentsAndTeachersFromDb();
        // await StoreStudentsOrTeachersInDb();
    }


    private static async Task AddStudentsOrTeachers(
        // Novo parâmetro para indicar o papel (role)
        User user, List<string> namesToAdd, string userRole)
    {
        var city =
            await _dataContextInUse.Cities
                .FirstOrDefaultAsync(c => c.Name == "Porto");
        var country =
            await _dataContextInUse.Countries
                .FirstOrDefaultAsync(c => c.Name == "Portugal");
        var countryOfNationality =
            await _dataContextInUse.Countries
                .Include(c => c.Nationality)
                .FirstOrDefaultAsync(c => c.Nationality.Name == "French");
        var birthplace =
            await _dataContextInUse.Countries
                .FirstOrDefaultAsync(c => c.Name == "France");
        var gender =
            await _dataContextInUse.Genders
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
                var student = await CreateUser<Student>(
                    firstName, lastName, address, email, postalCode,
                    cellPhone, dateOfBirth, idNumber, vatNumber,
                    user, city, country, countryOfNationality, birthplace,
                    gender, "Student");

                ListOfStudentsToAdd.Add(student);

                Console.WriteLine(
                    $"Created {userRole.ToLower()}: {firstName} {lastName}");

                Console.WriteLine("debug zone..");
            }
            else if (userRole.Equals(
                         "Teacher", StringComparison.OrdinalIgnoreCase))
            {
                var teacher = await CreateUser<Teacher>(
                    firstName, lastName, address, email, postalCode,
                    cellPhone, dateOfBirth, idNumber, vatNumber,
                    user, city, country, countryOfNationality, birthplace,
                    gender, "Teacher");

                ListOfTeachersToAdd.Add(teacher);
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


        // ------------------------------------------------------------------ //
        Console.WriteLine($"Created {namesToAdd.Count} {userRole.ToLower()}s");


        // ------------------------------------------------------------------ //
        Console.WriteLine($"Saving {userRole.ToLower()}s to the database...");


        // ------------------------------------------------------------------ //
        Console.WriteLine($"Saved {userRole.ToLower()}s to the database");


        // ------------------------------------------------------------------ //
        Console.WriteLine("debug zone..");


        // ------------------------------------------------------------------ //
        await _dataContextInUse.SaveChangesAsync();
    }


    private static async Task<T> CreateUser<T>(
        string firstName, string lastName, string address, string email,
        string postalCode,
        string cellPhone,
        DateTime dateOfBirth, string idNumber, string vatNumber,
        User user, City city, Country country,
        Country countryOfNationality, Country birthplace, Gender gender,
        // Novo parâmetro para indicar o papel (role)
        string userRole, string password = "Passw0rd"
        // Restrição genérica para permitir apenas classes
    ) where T : class
    {
        var newUser = new User
        {
            FirstName = firstName,
            LastName = lastName,
            Address = address,
            UserName = email,
            Email = email,
            PhoneNumber = cellPhone,
            WasDeleted = false,
            ProfilePhotoId = default
            // City = city,
            // CityId = city.Id,
            // Country = country,
            // CountryId = country.Id,
            // NationalityId = country.Nationality.Id,
        };

        // ------------------------------------------------------------------ //

        ListOfUsersToAdd.Add(newUser);


        // ------------------------------------------------------------------ //
        // store the user in the database
        try
        {
            var result =
                await _userHelper.GetUserByEmailAsync(newUser.Email);
            if (result != null)
                throw new Exception(
                    $"User {email} already exists in the database");

            var resultUser = await _userHelper.AddUserAsync(newUser, password);

            if (!resultUser.Succeeded)
                throw new Exception(
                    $"User {email} was not created: " +
                    $"{resultUser.Errors.FirstOrDefault()?.Description}");
        }
        catch (Exception ex)
        {
            // Log the exception or display a user-friendly error message.
        }


        // ------------------------------------------------------------------ //
        // check if the user was created and stored in the database
        try
        {
            await _userHelper.AddUserToRoleAsync(newUser, userRole);

            var result =
                await _userHelper.IsUserInRoleAsync(newUser, userRole);

            if (!result)
                throw new Exception(
                    $"User {email} was not added to role {userRole}");
        }
        catch (Exception ex)
        {
            // Log the exception or display a user-friendly error message.
        }


        // ------------------------------------------------------------------ //
        // check if the user was created and stored in the database
        try
        {
            var numberOfChanges = await _dataContextInUse.SaveChangesAsync();

            if (numberOfChanges > 0)
                // As alterações foram salvas com sucesso no banco de dados.
                Console.WriteLine("Changes saved successfully!");
            else
                // Nenhuma alteração foi salva no banco de dados.
                // Você pode optar por lançar uma exceção ou lidar com isso de alguma outra maneira.
                // Por exemplo:
                throw new Exception("No changes were saved to the database.");
        }
        catch (Exception ex)
        {
            // Lidar com a exceção ou registrar a mensagem de erro.
        }


        // ------------------------------------------------------------------ //
        Console.WriteLine(
            $"Creating {userRole.ToLower()}: {firstName} {lastName}");


        // ------------------------------------------------------------------ //

        Console.WriteLine("debug zone 1");

        // newUser = await _userHelper.GetUserByEmailAsync(newUser.Email);

        // ------------------------------------------------------------------ //
        // Create the specific entity (Student or Teacher) based on the generic type T
        var entity = Activator.CreateInstance<T>();
        // entity.GetType().GetProperty("User")?.SetValue(entity, newUser);
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
        entity.GetType().GetProperty("Gender")?.SetValue(entity, gender);
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


        // ------------------------------------------------------------------ //
        Console.WriteLine("Debug zone 2");

        return entity;
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
        var email =
            $"{sanitizedFirstName}.{sanitizedLastName}.cinel.pt@yopmail.com";


        // You can also convert the email to lowercase, if desired
        email = email.ToLower();


        return email;
    }


    private static async Task StoreStudentsOrTeachersUsersWithRoles(
        string password = "Passw0rd")
    {
        // Extract the emails from the lists of students and teachers
        var studentEmails =
            ListOfStudentsToAdd.Select(s => s.Email).ToList();
        var teacherEmails =
            ListOfTeachersToAdd.Select(t => t.Email).ToList();


        Console.WriteLine("Debug zone:");


        // Add students to the database and assign student role
        foreach (var studentUser in
                 ListOfUsersToAdd.Where(studentUser =>
                     !ExistingEmailsOfUsersFromDb.Contains(studentUser.Email)))
        {
            var result =
                await _userHelper.AddUserAsync(studentUser, password);

            if (result.Succeeded)
                await _userHelper.AddUserToRoleAsync(studentUser,
                    "Student");
        }


        // Add teachers to the database and assign teacher role
        foreach (var teacherUser in
                 ListOfUsersToAdd.Where(teacherUser =>
                     !ExistingEmailsOfUsersFromDb.Contains(teacherUser.Email)))
        {
            var result =
                await _userHelper.AddUserAsync(teacherUser, password);

            if (result.Succeeded)
                await _userHelper.AddUserToRoleAsync(teacherUser,
                    "Teacher");
        }

        // Save the changes to the student or teacher entity
        await _dataContextInUse.SaveChangesAsync();
    }


    private static async Task StoreStudentsOrTeachersInDb()
    {
        // ------------------------------------------------------------------ //
        Console.WriteLine("Debug zone 1");

        foreach (var student in ListOfStudentsToAdd)
        {
            var studentFromDb = _listOfStudentsFromDb
                .FirstOrDefault(s => s.Id == student.Id);

            if (studentFromDb != null) continue;

            var studentUser = _listOfUsersFromDb
                .FirstOrDefault(u => u.Email == student.Email);

            student.User = studentUser;

            _dataContextInUse.Students.Add(student);
        }

        await _dataContextInUse.SaveChangesAsync();


        foreach (var teacher in ListOfTeachersToAdd)
        {
            var teacherFromDb = _listOfTeachersFromDb
                .FirstOrDefault(t => t.Id == teacher.Id);

            if (teacherFromDb != null) continue;

            var teacherUser = _listOfUsersFromDb
                .FirstOrDefault(u => u.Email == teacher.Email);

            teacher.User = teacherUser;

            _dataContextInUse.Teachers.Add(teacher);
        }

        await _dataContextInUse.SaveChangesAsync();
    }
}