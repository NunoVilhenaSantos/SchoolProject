using System.Diagnostics;
using System.Drawing;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.ExtraEntities;
using SchoolProject.Web.Data.EntitiesMatrix;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Seeders;

public class SeedDb
{
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextSqLite _dataContextSqLite;

    private readonly ILogger<SeedDb> _logger;
    private readonly ILogger<SeedDbUsers> _loggerSeedDbUsers;
    private readonly ILogger<SeedDbPersons> _loggerSeedDbPersons;

    private readonly IWebHostEnvironment _hostingEnvironment;

    private readonly IUserHelper _userHelper;
    private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    // private readonly SeedDbUsers _seedDbUsers;
    // private readonly SeedDbPersons _seedDbPersons;
    // private readonly SeedDbSchoolClasses _seedDbSchoolClasses;

    public SeedDb(
        ILogger<SeedDb> logger,
        ILogger<SeedDbUsers> loggerSeedDbUsers,
        ILogger<SeedDbPersons> loggerSeedDbPersons,
        IUserHelper userHelper,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        SeedDbUsers seedDbUsers,
        SeedDbPersons seedDbPersons,
        SeedDbSchoolClasses seedDbSchoolClasses,
        IWebHostEnvironment hostingEnvironment,
        DataContextMsSql dataContextMsSql,
        DataContextMySql dataContextMySql,
        DataContextSqLite dataContextSqLite
    )
    {
        _logger = logger;
        _loggerSeedDbUsers = loggerSeedDbUsers;
        _loggerSeedDbPersons = loggerSeedDbPersons;

        _userHelper = userHelper;
        _userManager = userManager;
        _roleManager = roleManager;

        // _seedDbUsers = seedDbUsers;
        // _seedDbPersons = seedDbPersons;
        // _seedDbSchoolClasses = seedDbSchoolClasses;

        _hostingEnvironment = hostingEnvironment;

        _dataContextMsSql = dataContextMsSql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;
    }


    public async Task SeedAsync()
    {
        // ------------------------------------------------------------------ //
        // verify if the database exists and if not create it
        // ------------------------------------------------------------------ //
        await _dataContextMsSql.Database.MigrateAsync();
        await _dataContextMySql.Database.MigrateAsync();
        await _dataContextSqLite.Database.MigrateAsync();


        // ------------------------------------------------------------------ //
        // initialize SeedDbUsers with the user helper before been used
        // ------------------------------------------------------------------ //
        SeedDbUsers.Initialize(_userHelper, _loggerSeedDbUsers);

        Console.WriteLine("Seeding the database.", Color.Green);
        Console.WriteLine("Debug point.", Color.Red);

        // ------------------------------------------------------------------ //
        // adding roles for all users in the system
        // ------------------------------------------------------------------ //
        await SeedingRolesForUsers();


        // ------------------------------------------------------------------ //
        // adding the super users
        // ------------------------------------------------------------------ //
        await SeedingDataSuperUsers();


        // ------------------------------------------------------------------ //
        // adding the admin user
        // ------------------------------------------------------------------ //
        await SeedingDataAdminUsers();


        // ------------------------------------------------------------------ //
        // adding the Functionary user
        // ------------------------------------------------------------------ //
        await SeedingDataFunctionaryUsers();


        // ------------------------------------------------------------------ //
        // getting an admin to insert data that needs to have an user
        // ------------------------------------------------------------------ //
        var user = await _userHelper.GetUserByEmailAsync(
            "nuno.santos.26288@formandos.cinel.pt");


        // ------------------------------------------------------------------ //
        // adding countries and cities to the database
        // ------------------------------------------------------------------ //
        Debug.Assert(user != null, nameof(user) + " != null");
        await AddCountriesWithCitiesAndNationalities(user);


        // ------------------------------------------------------------------ //
        // adding genres to the database
        // ------------------------------------------------------------------ //
        await SeedingDataGenres(user);


        // ------------------------------------------------------------------ //
        // adding students and teachers to the database and also there user
        // ------------------------------------------------------------------ //
        // Em vez disso, crie uma instância da classe SeedDbPersons
        SeedDbPersons.Initialize(
            _userHelper, _loggerSeedDbPersons, _dataContextMsSql);
        await SeedDbPersons.AddingData();

        // var seedDbPersons = new SeedDbPersons(
        //     _userHelper, _loggerSeedDbPersons, _dataContextMsSql);

        // Em seguida, chame o método AddingData() na instância criada
        // await _seedDbPersons.AddingData();


        // ------------------------------------------------------------------ //
        // adding students and teachers to the database and also there user
        // ------------------------------------------------------------------ //
        SeedDbSchoolClasses.Initialize(
            _userHelper, _loggerSeedDbPersons, _dataContextMsSql);
        await SeedDbSchoolClasses.AddingData();


        // Em vez disso, crie uma instância da classe SeedDbSchoolClasses
        // var seedDbSchoolClasses = new SeedDbSchoolClasses(
        //     _userHelper, _loggerSeedDbPersons, _dataContextMsSql);
        // Em seguida, chame o método AddingData() na instância criada
        // await _seedDbSchoolClasses.AddingData();


        // verificar se existem os placeholders no sistema
        SeedDbPlaceHolders.Initialize(_hostingEnvironment);
        SeedDbPlaceHolders.AddPlaceHolders();


        await _dataContextMsSql.SaveChangesAsync();
    }


    private async Task SeedingDataGenres(User user)
    {
        var genresToAdd = new List<string>
        {
            "Male", "Female", "NonBinary", "PreferNotToSay"
        };

        var existingGenres =
            await _dataContextMsSql.Genres
                .Where(genre => genresToAdd.Contains(genre.Name))
                .Select(genre => genre.Name)
                .ToListAsync();

        var genresToCreate =
            genresToAdd.Except(existingGenres).ToList();

        var newGenres =
            genresToCreate.Select(genre => new Genre
            {
                Name = genre,
                IdGuid = Guid.NewGuid(),
                WasDeleted = false,
                CreatedAt = DateTime.Now,
                CreatedBy = user
            }).ToList();

        await _dataContextMsSql.Genres.AddRangeAsync(newGenres);
        await _dataContextMsSql.SaveChangesAsync();
    }


    private async Task SeedingDataSuperUsers()
    {
        await SeedDbUsers.AddUsers(
            "Nuno", "Vilhena Santos",
            "nunovilhenasantos@msn.com",
            "Calle Luna", "SuperUser");

        await SeedDbUsers.AddUsers(
            "Nuno", "Santos",
            "nuno.santos.26288@formandos.cinel.pt",
            "Calle Luna", "SuperUser");

        await SeedDbUsers.AddUsers(
            "Rafael", "Santos",
            "rafael.santos@cinel.pt",
            "Calle Luna", "SuperUser");
    }


    private async Task SeedingDataAdminUsers()
    {
        await SeedDbUsers.AddUsers(
            "Jorge", "Pinto",
            "jorge.pinto.28720@formandos.cinel.pt",
            "Calle Luna", "Admin");

        await SeedDbUsers.AddUsers(
            "Ruben", "Correia",
            "ruben.corrreia.28257@formandos.cinel.pt",
            "Calle Luna", "Admin");

        await SeedDbUsers.AddUsers(
            "Tatiane", "Avellar",
            "tatiane.avellar.24718@formandos.cinel.pt",
            "Calle Luna", "Admin");
    }


    private async Task SeedingDataFunctionaryUsers()
    {
        await SeedDbUsers.AddUsers(
            "Licinio", "Rosario",
            "licinio.do.rosario@formandos.cinel.pt",
            "Calle Luna", "Functionary");

        await SeedDbUsers.AddUsers(
            "Joel", "Rangel",
            "joel.rangel.22101@formandos.cinel.pt",
            "Calle Luna", "Functionary");

        await SeedDbUsers.AddUsers(
            "Diogo", "Alves",
            "diogo.alves.28645@formandos.cinel.pt",
            "Calle Luna", "Functionary");
    }


    private async Task SeedingRolesForUsers()
    {
        var rolesToCreate = new List<string>
        {
            "SuperUser", "Admin", "Functionary",
            "Student", "Teacher", "Parent",
            "User"
        };

        var existingRoles =
            await _roleManager.Roles
                .Where(role => rolesToCreate.Contains(role.Name))
                .ToListAsync();

        var rolesToAdd = rolesToCreate
            .Except(existingRoles.Select(role => role.Name))
            .ToList();


        foreach (var role in rolesToAdd) await CreateRoleAsync(role);

        await _dataContextMsSql.SaveChangesAsync();
    }

    private async Task<IdentityResult> CreateRoleAsync(string role)
    {
        return await _roleManager.CreateAsync(new IdentityRole(role));
    }


    private async Task AddCountriesWithCitiesAndNationalities(User createdBy)
    {
        if (_dataContextMsSql.Countries.Any()) return;

        var countryCityData = new Dictionary<string, List<string>>
        {
            {
                "Angola", new List<string>
                {
                    "Luanda", "Lobito", "Benguela", "Lubango", "Huambo",
                    "Namibe", "Lubango", "Alto Catumbela", "Cabinda", "Caxito",
                    "Cuito", "Dundo", "Malanje", "Menongue", "Namibe",
                    "N'dalatando", "Ondjiva", "Saurimo", "Soio", "Sumbe",
                    "Uíge", "Xangongo"
                }
            },
            {
                "Portugal", new List<string>
                {
                    "Lisboa", "Porto", "Coimbra", "Faro", "Braga", "Aveiro",
                    "Évora", "Funchal", "Viseu", "Viana do Castelo", "Beja",
                    "Bragança", "Castelo Branco", "Guarda", "Leiria",
                    "Portalegre", "Santarém", "Setúbal", "Vila Real", "Angra"
                }
            },
            {
                "Spain", new List<string>
                {
                    "Madrid", "Salamanca", "Sevilha", "Valencia", "Barcelona",
                    "Bilbao", "Santiago de Compostela", "Toledo", "Córdoba",
                    "Granada", "Málaga", "Zaragoza", "Alicante", "Cádiz"
                }
            },
            {
                "France", new List<string>
                {
                    "Paris", "Lyon", "Marselha", "Nante", "Estrasbourgo",
                    "Bordeaux", "Toulouse", "Lille", "Nice", "Rennes",
                    "Reims", "Saint-Étienne", "Toulon", "Le Havre"
                }
            },
            {
                "Brazil", new List<string>
                {
                    "Rio de Janeiro", "São Paulo", "Salvador", "Brasília",
                    "Fortaleza", "Belo Horizonte", "Manaus", "Curitiba",
                    "Recife", "Porto Alegre", "Belém", "Goiânia", "Guarulhos"
                }
            },
            {
                "Cuba", new List<string>
                {
                    "Havana", "Santiago de Cuba", "Camagüey", "Holguín",
                    "Guantánamo", "Santa Clara", "Las Tunas", "Bayamo",
                    "Cienfuegos", "Pinar del Río", "Matanzas", "Ciego de Ávila"
                }
            },
            {
                "Mexico", new List<string>
                {
                    "Mexico City", "Guadalajara", "Monterrey", "Puebla",
                    "Toluca", "Tijuana", "León", "Ciudad Juárez", "La Laguna",
                    "San Luis Potosí", "Mérida", "Mexicali", "Aguascalientes",
                    "Cuernavaca", "Acuña"
                }
            },
            {
                "Russia", new List<string>
                {
                    "Moscovo", "São Petersburgo", "Novosibirsk",
                    "Ecaterimburgo", "Níjni Novgorod", "Samara", "Omsk",
                    "Cazã", "Cheliabinsk", "Rostov do Don", "Ufá", "Volgogrado"
                }
            }
        };


        foreach (var countryEntry in countryCityData)
        {
            var countryName = countryEntry.Key;
            var cityNames = countryEntry.Value;

            if (_dataContextMsSql.Countries
                .Any(c => c.Name == countryName))
                continue;

            var cities = CreateCities(cityNames, createdBy);
            var nationalityName = GetNationalityName(countryName);
            var nationality = new Nationality
            {
                Name = nationalityName,
                IdGuid = Guid.NewGuid(),
                CreatedBy = createdBy
            };
            var country = new Country
            {
                Name = countryName,
                Cities = cities,
                WasDeleted = false,
                Nationality = nationality,
                IdGuid = Guid.NewGuid(),
                CreatedBy = createdBy
            };

            await _dataContextMsSql.Countries.AddAsync(country);
        }

        await _dataContextMsSql.SaveChangesAsync();
    }


    private List<City> CreateCities(List<string> cityNames, User createdBy)
    {
        return cityNames.Select(
            cityName => new City
            {
                Name = cityName,
                WasDeleted = false,
                IdGuid = Guid.NewGuid(),
                CreatedBy = createdBy
            }
        ).ToList();
    }


    private static string GetNationalityNameInPortugues(string countryName)
    {
        return countryName switch
        {
            "Angola" => "Angolana",
            "Portugal" => "Portuguesa",
            "Spain" => "Espanhola",
            "France" => "Francesa",
            "Brazil" => "Brasileira",
            "Cuba" => "Cubana",
            "Mexico" => "Mexicana",
            "Russia" => "Russa",
            _ => countryName + "n"
        };
    }


    private static string GetNationalityName(string countryName)
    {
        return countryName switch
        {
            "Angola" => "Angolan",
            "Portugal" => "Portuguese",
            "Spain" => "Spanish",
            "France" => "French",
            "Brazil" => "Brazilian",
            "Cuba" => "Cuban",
            "Mexico" => "Mexican",
            "Russia" => "Russian",
            _ => countryName + "n"
        };
    }
}