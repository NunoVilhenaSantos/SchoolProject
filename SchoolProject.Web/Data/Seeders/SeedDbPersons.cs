using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
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


    internal static async Task AddingData()
    {
        var user = await _userHelper.GetUserByEmailAsync(
            "nuno.santos.26288@formandos.cinel.pt");


        if (!_dataContextMssql.Students.Any())
        {
            await AddStudent(
                "Juan", "Zuluaga",
                "Calle Luna", user);
            await AddStudent(
                "Joaquim", "Alvenaria",
                "Calle Sol", user);
            await AddStudent(
                "Alberto", "Domingues",
                "Calle Luna", user);
            await AddStudent(
                "Mariana", "Alvarez",
                "Calle Sol", user);
            await AddStudent(
                "Lucia", "Liu",
                "Calle Luna", user);
            await AddStudent(
                "Renee", "Arriaga",
                "Calle Sol", user);
            await AddStudent(
                "Marcia", "Zuluaga",
                "Calle Luna", user);
            await AddStudent(
                "Ernesto", "Guevara",
                "Calle Sol", user);
            await AddStudent(
                "El", "Che",
                "Calle Luna", user);
            await AddStudent(
                "Claudia", "Arroz",
                "Calle Sol", user);
        }

        user = await _userHelper.GetUserByEmailAsync(
            "nuno.santos.26288@formandos.cinel.pt");


        if (!_dataContextMssql.Teachers.Any())
        {
            await AddTeacher(
                "Roberto", "Rossellini",
                "Calle Luna", user);
            await AddTeacher(
                "Giuseppe", "Tornatore",
                "Calle Luna", user);
            await AddTeacher(
                "Federico", "Fellini",
                "Rimini", user);
            await AddTeacher(
                "Ingrid", "Bergman",
                "Calle Sol", user);
            await AddTeacher(
                "Gina", "Lollobrigida",
                "Calle Sol", user);
            await AddTeacher(
                "Isabella", "Rossellini",
                "Calle Luna", user);
            await AddTeacher(
                "Monica", "Bellucci",
                "Calle Sol", user);
            await AddTeacher(
                "Giovanna", "Ralli",
                "Calle Luna", user);
            await AddTeacher(
                "Valeria", "Golino",
                "Calle Sol", user);
            await AddTeacher(
                "Sophia", "Loren",
                "Calle Luna", user);
            await AddTeacher(
                "Claudia", "Cardinale",
                "Calle Sol", user);
        }
    }


    internal static async Task AddStudent(
        string firstName, string lastName, string address, User user)
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

        const string userRole = "Student";

        var studentWithRole =
            _dataContextMssql.Students.Add(new Student
                {
                    User = await SeedDbUsers.CheckUserAsync(
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
                    City = await _dataContextMssql.Cities
                        .FindAsync("Porto") ?? new City
                    {
                        Name = "Porto",
                        Id = 0,
                        IdGuid = new Guid(),
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user,
                        WasDeleted = false
                    },
                    Country = await _dataContextMssql.Countries
                        .FindAsync("Portugal") ?? new Country
                    {
                        Name = "Portugal",
                        Nationality = await _dataContextMssql.Nationalities
                            .FindAsync("Português") ?? new Nationality
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
                    Genre = await _dataContextMssql.Genres
                        .FindAsync("Female") ?? new Genre
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
                    CountryOfNationality = await _dataContextMssql.Countries
                        .FindAsync("Portugal") ?? new Country
                    {
                        Name = "Portugal",
                        Nationality = await _dataContextMssql.Nationalities
                            .FindAsync("Português") ?? new Nationality
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
                    Birthplace = await _dataContextMssql.Countries
                        .FindAsync("França") ?? new Country
                    {
                        Name = "França",
                        Nationality = await _dataContextMssql.Nationalities
                            .FindAsync("Françês") ?? new Nationality
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
                    Id = 0,
                    IdGuid = new Guid(),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = user
                }
            );


        var newUser = await _dataContextMssql.Users
            .FirstOrDefaultAsync(u =>
                u.Email == studentWithRole.Entity.Email);

        var role = await _dataContextMssql.Roles
            .FirstOrDefaultAsync(r =>
                r.Name == userRole);

        _dataContextMssql.UserRoles.Add(
            new IdentityUserRole<string>
            {
                UserId = newUser?.Id ?? string.Empty,
                RoleId = role?.Id ?? string.Empty
            });
    }


    internal static async Task AddTeacher(
        string firstName, string lastName, string address, User user)
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

        const string userRole = "Teacher";

        var teacherWithRole =
            _dataContextMssql.Teachers.Add(new Teacher
                {
                    User = await SeedDbUsers.CheckUserAsync(
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
                    City = await _dataContextMssql.Cities
                        .FindAsync("Porto") ?? new City
                    {
                        Name = "Porto",
                        Id = 0,
                        IdGuid = new Guid(),
                        CreatedAt = DateTime.UtcNow,
                        CreatedBy = user,
                        WasDeleted = false
                    },
                    Country = await _dataContextMssql.Countries
                        .FindAsync("Portugal") ?? new Country
                    {
                        Name = "Portugal",
                        Nationality = await _dataContextMssql.Nationalities
                            .FindAsync("Português") ?? new Nationality
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
                    Genre = await _dataContextMssql.Genres
                        .FindAsync("Female") ?? new Genre
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
                    CountryOfNationality = await _dataContextMssql.Countries
                        .FindAsync("Portugal") ?? new Country
                    {
                        Name = "Portugal",
                        Nationality = await _dataContextMssql.Nationalities
                            .FindAsync("Português") ?? new Nationality
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
                    Birthplace = await _dataContextMssql.Countries
                        .FindAsync("França") ?? new Country
                    {
                        Name = "França",
                        Nationality = await _dataContextMssql.Nationalities
                            .FindAsync("Françês") ?? new Nationality
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
                    Id = 0,
                    IdGuid = new Guid(),
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = user
                }
            );


        var newUser = await _dataContextMssql.Users
            .FirstOrDefaultAsync(u =>
                u.Email == teacherWithRole.Entity.Email);

        var role = await _dataContextMssql.Roles
            .FirstOrDefaultAsync(r =>
                r.Name == userRole);

        _dataContextMssql.UserRoles.Add(
            new IdentityUserRole<string>
            {
                UserId = newUser?.Id ?? string.Empty,
                RoleId = role?.Id ?? string.Empty
            });
    }


    public static DateTime GenerateRandomDateOfBirth()
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
}