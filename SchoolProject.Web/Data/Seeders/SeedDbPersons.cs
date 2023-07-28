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

public static class SeedDbPersons
{
    private static Random _random;
    private static IUserHelper _userHelper;
    private static DataContextMsSql _dataContextMssql;


    // Add a constructor to receive IUserHelper through dependency injection
    public static void Initialize(
        IUserHelper userHelper,
        DataContextMsSql dataContextMssql
    )
    {
        _random = new Random();
        _userHelper = userHelper;
        _dataContextMssql = dataContextMssql;
    }


    internal static async Task AddingData()
    {
        var user = await _userHelper.GetUserByEmailAsync(
            "nuno.santos.26288@formandos.cinel.pt");


        Console.WriteLine(
            "Seeding the users table with students and teachers...");


        if (!_dataContextMssql.Students.Any())
        {
            await AddStudent(
                "Student 1", "Zuluaga", "Calle Luna", user);
            await AddStudent(
                "Student 2", "Alvenaria", "Calle Sol", user);
            await AddStudent(
                "Student 3", "Domingues", "Calle Luna", user);
            await AddStudent(
                "Student 4", "Alvarez", "Calle Sol", user);
            await AddStudent(
                "Student 5", "Liu", "Calle Luna", user);
            await AddStudent(
                "Student 6", "Arriaga", "Calle Sol", user);
            await AddStudent(
                "Student 7", "Zuluaga", "Calle Luna", user);
            await AddStudent(
                "Student 8", "Guevara", "Calle Sol", user);
            await AddStudent(
                "Student 9", "Che", "Calle Luna", user);
            await AddStudent(
                "Student 10", "Arroz", "Calle Sol", user);
        }

        user = await _userHelper.GetUserByEmailAsync(
            "nuno.santos.26288@formandos.cinel.pt");


        if (!_dataContextMssql.Teachers.Any())
        {
            await AddTeacher(
                "Roberto", "Rossellini", "Calle Luna", user);
            await AddTeacher(
                "Giuseppe", "Tornatore", "Calle Luna", user);
            await AddTeacher(
                "Federico", "Fellini", "Rimini", user);
            await AddTeacher(
                "Ingrid", "Bergman", "Calle Sol", user);
            await AddTeacher(
                "Gina", "Lollobrigida", "Calle Sol", user);
            await AddTeacher(
                "Isabella", "Rossellini", "Calle Luna", user);
            await AddTeacher(
                "Monica", "Bellucci", "Calle Sol", user);
            await AddTeacher(
                "Giovanna", "Ralli", "Calle Luna", user);
            await AddTeacher(
                "Valeria", "Golino", "Calle Sol", user);
            await AddTeacher(
                "Sophia", "Loren", "Calle Luna", user);
            await AddTeacher(
                "Claudia", "Cardinale", "Calle Sol", user);
        }
    }


    private static async Task AddStudent(
        string firstName, string lastName, string address, User user,
        string userRole = "Student", string password = "123456")
    {
        var document = _random.Next(100000, 999999999).ToString();
        var fixedPhone = _random.Next(1000000, 99999999).ToString();
        var cellPhone = _random.Next(1000000, 99999999).ToString();
        var addressFull = address + ", " + _random.Next(1, 9999);
        var email = $"{firstName}.{lastName}@mail.pt";
        var identificationNumber = _random.Next(100000, 999999999).ToString();
        var vatNumber = _random.Next(100000, 999999999).ToString();

        var dateOfBirth = GenerateRandomDateOfBirth();

        var postalCode =
            _random.Next(1000, 9999) + "-" + _random.Next(100, 999);

        // const string userRole = "Student";


        var city = await _dataContextMssql.Cities
            .FirstOrDefaultAsync(c => c.Name == "Porto");

        var country = await _dataContextMssql.Countries
            .FirstOrDefaultAsync(c => c.Name == "Portugal");

        var nationality = await _dataContextMssql.Nationalities
            .FirstOrDefaultAsync(n => n.Name == "Português");

        var genre = await _dataContextMssql.Genres
            .FirstOrDefaultAsync(g => g.Name == "Female");

        var countryOfNationality = await _dataContextMssql.Countries
            .FirstOrDefaultAsync(c => c.Name == "Portugal");

        var countryOfNationalityNationality = await _dataContextMssql
            .Nationalities
            .FirstOrDefaultAsync(n => n.Name == "Português");

        var birthplace = await _dataContextMssql.Countries
            .FirstOrDefaultAsync(c => c.Name == "França");

        var birthplaceNationality = await _dataContextMssql.Nationalities
            .FirstOrDefaultAsync(n => n.Name == "Françês");


        var studentWithRole =
            _dataContextMssql.Students.Add(new Student
                {
                    User = await SeedDbUsers.VerifyUserAsync(
                        firstName,
                        lastName,
                        email,
                        email,
                        fixedPhone,
                        userRole,
                        document,
                        addressFull
                    ),

                    FirstName = firstName,
                    LastName = lastName,
                    Address = addressFull,
                    PostalCode = postalCode,

                    City = city ?? new City
                    {
                        Name = "Porto",
                        // Id = 0,
                        IdGuid = new Guid(),
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user,
                        WasDeleted = false
                    },

                    Country = country ?? new Country
                    {
                        Name = "Portugal",
                        Nationality = nationality ?? new Nationality
                        {
                            Name = "Português",
                            IdGuid = new Guid(),
                            WasDeleted = false,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = user
                        },
                        IdGuid = new Guid(),
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user,
                        WasDeleted = false
                    },

                    MobilePhone = cellPhone,
                    Email = email,
                    Active = true,

                    Genre = genre ?? new Genre
                    {
                        Name = "Female",
                        IdGuid = new Guid(),
                        WasDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user,
                        // TODO: Set the UpdatedAt property to null
                        // Set the UpdatedAt property to the current date and time
                        UpdatedAt = DateTime.UtcNow
                    },

                    DateOfBirth = dateOfBirth,
                    IdentificationNumber = identificationNumber,
                    IdentificationType = "CC",
                    ExpirationDateIdentificationNumber = default,
                    TaxIdentificationNumber = vatNumber,

                    CountryOfNationality = countryOfNationality ?? new Country
                    {
                        Name = "Portugal",
                        Nationality =
                            countryOfNationalityNationality ?? new Nationality
                            {
                                Name = "Português",
                                IdGuid = new Guid(),
                                WasDeleted = false,
                                CreatedAt =
                                    DateTime.UtcNow,
                                CreatedBy = user
                            },
                        IdGuid = new Guid(),
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user,
                        WasDeleted = false
                    },

                    Birthplace = birthplace ?? new Country
                    {
                        Name = "França",
                        Nationality = birthplaceNationality ?? new Nationality
                        {
                            Name = "Françês",
                            IdGuid = new Guid(),
                            WasDeleted = false,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = user
                        },
                        IdGuid = new Guid(),
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user,
                        WasDeleted = false
                    },

                    EnrollDate = DateTime.UtcNow,
                    // Id = 0,
                    IdGuid = new Guid(),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = user,

                    // todo: Set the UpdatedAt property to null
                    // the 'UpdatedAt' property
                    UpdatedAt = DateTime.UtcNow
                }
            );


        await StoreStudentsOrTeachersWithRoles(studentWithRole, userRole);
    }


    private static async Task AddTeacher(
        string firstName, string lastName, string address,
        User user,
        string userRole = "Teacher", string password = "123456")
    {
        var document = _random.Next(100000, 999999999).ToString();
        var fixedPhone = _random.Next(1000000, 99999999).ToString();
        var cellPhone = _random.Next(1000000, 99999999).ToString();
        var addressFull = address + ", " + _random.Next(1, 9999);
        var email = $"{firstName}.{lastName}@mail.pt";
        var identificationNumber = _random.Next(100000, 999999999).ToString();
        var vatNumber = _random.Next(100000, 999999999).ToString();

        var dateOfBirth = GenerateRandomDateOfBirth();

        var postalCode =
            _random.Next(1000, 9999) + "-" + _random.Next(100, 999);

        // const string userRole = "Teacher";


        var city = await _dataContextMssql.Cities
            .FirstOrDefaultAsync(c => c.Name == "Porto");

        var country = await _dataContextMssql.Countries
            .FirstOrDefaultAsync(c => c.Name == "Portugal");

        var nationality = await _dataContextMssql.Nationalities
            .FirstOrDefaultAsync(n => n.Name == "Português");

        var genre = await _dataContextMssql.Genres
            .FirstOrDefaultAsync(g => g.Name == "Female");

        var countryOfNationality = await _dataContextMssql.Countries
            .FirstOrDefaultAsync(c => c.Name == "Portugal");

        var countryOfNationalityNationality = await _dataContextMssql
            .Nationalities
            .FirstOrDefaultAsync(n => n.Name == "Português");

        var birthplace = await _dataContextMssql.Countries
            .FirstOrDefaultAsync(c => c.Name == "França");

        var birthplaceNationality = await _dataContextMssql.Nationalities
            .FirstOrDefaultAsync(n => n.Name == "Françês");


        var teacherWithRole =
            _dataContextMssql.Teachers.Add(new Teacher
                {
                    User = await SeedDbUsers.VerifyUserAsync(
                        firstName,
                        lastName,
                        email,
                        email,
                        fixedPhone,
                        userRole,
                        document,
                        addressFull
                    ),
                    FirstName = firstName,
                    LastName = lastName,
                    Address = addressFull,
                    PostalCode = postalCode,

                    City = city ?? new City
                    {
                        Name = "Porto",
                        // Id = 0,
                        IdGuid = new Guid(),
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user,
                        WasDeleted = false
                    },

                    Country = country ?? new Country
                    {
                        Name = "Portugal",
                        Nationality = nationality ?? new Nationality
                        {
                            Name = "Português",
                            IdGuid = new Guid(),
                            WasDeleted = false,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = user
                        },
                        IdGuid = new Guid(),
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user,
                        WasDeleted = false
                    },

                    MobilePhone = cellPhone,
                    Email = email,
                    Active = true,

                    Genre = genre ?? new Genre
                    {
                        Name = "Female",
                        IdGuid = new Guid(),
                        WasDeleted = false,
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user
                    },

                    DateOfBirth = dateOfBirth,
                    IdentificationNumber = identificationNumber,
                    IdentificationType = "BI",
                    ExpirationDateIdentificationNumber = default,
                    TaxIdentificationNumber = vatNumber,

                    CountryOfNationality = countryOfNationality ?? new Country
                    {
                        Name = "Portugal",
                        Nationality =
                            countryOfNationalityNationality ?? new Nationality
                            {
                                Name = "Português",
                                IdGuid = new Guid(),
                                WasDeleted = false,
                                CreatedAt = DateTime.UtcNow,
                                CreatedBy = user
                            },
                        IdGuid = new Guid(),
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user,
                        WasDeleted = false
                    },

                    Birthplace = birthplace ?? new Country
                    {
                        Name = "França",
                        Nationality = birthplaceNationality ?? new Nationality
                        {
                            Name = "Françês",
                            IdGuid = new Guid(),
                            WasDeleted = false,
                            CreatedAt = DateTime.UtcNow,
                            CreatedBy = user
                        },
                        IdGuid = new Guid(),
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user,
                        WasDeleted = false
                    },

                    EnrollDate = DateTime.UtcNow,
                    // Id = 0,
                    IdGuid = new Guid(),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = user
                }
            );


        await StoreStudentsOrTeachersWithRoles(teacherWithRole, userRole);
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