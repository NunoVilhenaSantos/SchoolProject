using System.Diagnostics;
using System.Drawing;
using System.Security.Claims;
using System.Transactions;
using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.Entities.Countries;
using SchoolProject.Web.Data.Entities.OtherEntities;
using SchoolProject.Web.Data.Entities.Users;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Seeders;

/// <summary>
///     Data seeder for the database.
/// </summary>
public class SeedDb
{
    //private readonly DataContextMsSql _dataContextInUse;
    private readonly DataContextMySql _dataContextInUse;
    //private readonly DataContextSqLite _dataContextInUse;

    // private readonly DcMsSqlLocal _msSqlLocal;
    // private readonly DCMySqlLocal _mySqlLocal;

    // private readonly DcMsSqlOnline _msSqlOnline;
    // private readonly DCMySqlOnline _mySqlOnline;

    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextSqLite _dataContextSqLite;


    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly ILogger<SeedDbSchoolClasses> _loggerSeedDbSCs;
    private readonly ILogger<SeedDbStudentsAndTeachers> _loggerSeedDbSTs;

    private readonly ILogger<SeedDbUsers> _loggerSeedDbUsers;
    private readonly RoleManager<IdentityRole> _roleManager;


    private readonly IUserHelper _userHelper;
    private readonly UserManager<User> _userManager;


    public SeedDb(
        // ILogger<SeedDb> logger,
        ILogger<SeedDbUsers> loggerSeedDbUsers,
        ILogger<SeedDbSchoolClasses> loggerSeedDbSCs,
        ILogger<SeedDbStudentsAndTeachers> loggerSeedDbSTs,
        IUserHelper userHelper,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IWebHostEnvironment hostingEnvironment,
        IConfiguration configuration,
        DataContextMsSql dataContextMsSql,
        DataContextMySql dataContextMySql,
        DataContextSqLite dataContextSqLite,
        DataContextMySql dataContextInUse
        // DcMsSqlLocal msSqlLocal,
        // DCMySqlLocal mySqlLocal,
        // DcMsSqlOnline msSqlOnline,
        // DCMySqlOnline mySqlOnline
    )
    {
        _loggerSeedDbSCs = loggerSeedDbSCs;
        _loggerSeedDbSTs = loggerSeedDbSTs;
        _loggerSeedDbUsers = loggerSeedDbUsers;

        _userHelper = userHelper;
        _userManager = userManager;
        _roleManager = roleManager;

        _hostingEnvironment = hostingEnvironment;

        _dataContextMsSql = dataContextMsSql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;

        _dataContextInUse = dataContextInUse;

        // _msSqlLocal = msSqlLocal;
        // _mySqlLocal = mySqlLocal;

        // _msSqlOnline = msSqlOnline;
        // _mySqlOnline = mySqlOnline;
    }


    public async Task SeedAsync()
    {
        // ------------------------------------------------------------------ //
        // _dataContextInUse = _dataContextMySql;

        // ------------------------------------------------------------------ //
        // verify if the database exists and if not create it
        // ------------------------------------------------------------------ //
        await _dataContextMsSql.Database.MigrateAsync();
        // await _msSqlLocal.Database.MigrateAsync();
        // await _msSqlOnline.Database.MigrateAsync();

        await _dataContextMySql.Database.MigrateAsync();
        // await _mySqlLocal.Database.MigrateAsync();
        // await _mySqlOnline.Database.MigrateAsync();

        // await _dataContextSqLite.Database.MigrateAsync();

        await _dataContextInUse.Database.MigrateAsync();


        // ------------------------------------------------------------------ //


        // ------------------------------------------------------------------ //
        // initialize SeedDbUsers with the user helper before been used
        // ------------------------------------------------------------------ //
        SeedDbUsers.Initialize(
            _userHelper, _loggerSeedDbUsers, _dataContextInUse);

        Console.WriteLine("Seeding the database.", Color.Green);
        Console.WriteLine(
            _dataContextInUse.Database.GetConnectionString(), Color.Green);
        Console.WriteLine(
            _dataContextInUse.Database.GetDbConnection().DataSource,
            Color.Green);
        Console.WriteLine("Debug point.", Color.Red);


        // ------------------------------------------------------------------ //
        // adding roles for all users in the system
        // ------------------------------------------------------------------ //
        await SeedingRolesForUsers();
        await _dataContextInUse.SaveChangesAsync();
        //await _dataContextInUse.Database.CommitTransactionAsync();


        // ------------------------------------------------------------------ //
        // adding the super users
        // ------------------------------------------------------------------ //
        await SeedingDataSuperUsers();
        await _dataContextInUse.SaveChangesAsync();


        // ------------------------------------------------------------------ //
        // adding the admin user
        // ------------------------------------------------------------------ //
        await SeedingDataAdminUsers();
        await _dataContextInUse.SaveChangesAsync();


        // ------------------------------------------------------------------ //
        // adding the Functionary user
        // ------------------------------------------------------------------ //
        await SeedingDataFunctionaryUsers();
        await _dataContextInUse.SaveChangesAsync();


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
        await _dataContextInUse.SaveChangesAsync();


        // ------------------------------------------------------------------ //
        // adding genres to the database
        // ------------------------------------------------------------------ //
        await SeedingDataGenders(user);
        await _dataContextInUse.SaveChangesAsync();


        // ------------------------------------------------------------------ //
        // adding students and teachers to the database and also there user
        // ------------------------------------------------------------------ //
        SeedDbStudentsAndTeachers.Initialize(_userHelper, _dataContextInUse);
        await SeedDbStudentsAndTeachers.AddingData(user);
        await _dataContextInUse.SaveChangesAsync();


        // ------------------------------------------------------------------ //
        // adding students and teachers to the database and also there user
        // ------------------------------------------------------------------ //
        SeedDbSchoolClasses.Initialize(_dataContextInUse);
        await SeedDbSchoolClasses.AddingData(user);
        await _dataContextInUse.SaveChangesAsync();


        // ------------------------------------------------------------------ //
        // adding courses to teachers to the database
        // ------------------------------------------------------------------ //
        // SeedDbTeachersWithCourses.Initialize(_dataContextInUse);
        await SeedDbTeachersWithCourses.AddingData(_dataContextInUse, user);
        await _dataContextInUse.SaveChangesAsync();

        Console.WriteLine("Debug point.", Color.Red);


        // ------------------------------------------------------------------ //
        // adding courses to teachers to the database
        // ------------------------------------------------------------------ //
        // SeedDbSchoolClassesWithCourses.Initialize(_dataContextInUse);
        await SeedDbSchoolClassesWithCourses.AddingData(
            user, _dataContextInUse);
        await _dataContextInUse.SaveChangesAsync();

        Console.WriteLine("Debug point.", Color.Red);


        // ------------------------------------------------------------------ //
        // adding students to school-classes into the database
        // ------------------------------------------------------------------ //
        // SeedDbStudentsWithSchoolClasses.Initialize(_dataContextInUse);
        await SeedDbStudentsWithSchoolClasses.AddingData(
            user, _dataContextInUse);
        await _dataContextInUse.SaveChangesAsync();

        Console.WriteLine("Debug point.", Color.Red);


        // lista de schoolclasses no estudante e vice-versa


        // ------------------------------------------------------------------ //
        Console.WriteLine("Seeding the database finished.", Color.Green);
        // ------------------------------------------------------------------ //


        // ------------------------------------------------------------------ //
        // verificar se existem os placeholders no sistema
        // ------------------------------------------------------------------ //
        SeedDbPlaceHolders.Initialize(_hostingEnvironment);
        SeedDbPlaceHolders.AddPlaceHolders();


        // ------------------------------------------------------------------ //
        // saving all the changes to the database
        // ------------------------------------------------------------------ //
        await _dataContextInUse.SaveChangesAsync();

        // ------------------------------------------------------------------ //
        try
        {
            // Realiza as transações necessárias e
            // salva as alterações no banco de dados
            await _dataContextInUse.SaveChangesAsync();

            // Enquanto houver uma transação ativa,
            // aguarde e verifique novamente
            while (_dataContextInUse.Database.GetEnlistedTransaction() != null)
                // Aguardar um período de tempo
                // Exemplo: 5 segundos
                await Task.Delay(TimeSpan.FromSeconds(5));

            // Todas as transações estão concluídas,
            // então chama Dispose para liberar o contexto
            await _dataContextInUse.DisposeAsync();
        }
        catch (Exception ex)
        {
            // Lida com exceções, se necessário
        }
        finally
        {
            // Se você não quiser mais que o contexto seja reutilizado,
            // chame Dispose novamente
            await _dataContextInUse.DisposeAsync();
        }
    }


    private async Task SeedingDataGenders(User user)
    {
        var gendersToAdd = new List<string>
        {
            "Male", "Female", "NonBinary", "PreferNotToSay", "Other",
            "Transgender", "Intersex", "GenderFluid", "GenderQueer",
            "Agender", "Androgyne", "Bigender", "Demigender", "Genderless"
        };

        var existingGenders =
            await _dataContextInUse.Genders
                .Where(g => gendersToAdd.Contains(g.Name))
                .Select(g => g.Name)
                .ToListAsync();

        var gendersToCreate =
            gendersToAdd.Except(existingGenders).ToList();

        var newGender =
            gendersToCreate.Select(gender => new Gender
            {
                Name = gender,
                IdGuid = Guid.NewGuid(),
                ProfilePhotoId = default,
                WasDeleted = false,
                CreatedAt = DateTime.Now,
                CreatedBy = user
            }).ToList();

        await _dataContextInUse.Genders.AddRangeAsync(newGender);
        await _dataContextInUse.SaveChangesAsync();
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

        foreach (var role in rolesToAdd)
        {
            await CreateRoleAsync(role);

            // Associar a função com as reivindicações apropriadas
            switch (role)
            {
                case "SuperUser":
                    await AddClaimToRoleAsync(role, "IsAdmin");
                    await AddClaimToRoleAsync(role, "IsFunctionary");
                    await AddClaimToRoleAsync(role, "IsStudent");
                    await AddClaimToRoleAsync(role, "IsTeacher");
                    await AddClaimToRoleAsync(role, "IsParent");
                    await AddClaimToRoleAsync(role, "IsUser");

                    // ... outras reivindicações conforme necessário
                    break;

                case "Admin":
                    await AddClaimToRoleAsync(role, "IsAdmin");
                    // ...
                    break;

                case "Functionary":
                    await AddClaimToRoleAsync(role, "IsFunctionary");
                    // ...
                    break;

                case "Student":
                    await AddClaimToRoleAsync(role, "IsStudent");
                    // ...
                    break;

                case "Teacher":
                    await AddClaimToRoleAsync(role, "IsTeacher");
                    // ...
                    break;

                case "Parent":
                    await AddClaimToRoleAsync(role, "IsParent");
                    // ...
                    break;

                case "User":
                    await AddClaimToRoleAsync(role, "IsUser");
                    // ...
                    break;
            }
        }

        await _dataContextInUse.SaveChangesAsync();
    }


    private async Task CreateRoleAsync(string role)
    {
        await _roleManager.CreateAsync(new IdentityRole(role));
    }


    private async Task AddClaimToRoleAsync(string roleName, string claimType)
    {
        var role = await _roleManager.FindByNameAsync(roleName);

        if (role != null)
        {
            var claim = new Claim(claimType, "true");
            await _roleManager.AddClaimAsync(role, claim);
        }
    }


    private async Task AddCountriesWithCitiesAndNationalities(User createdBy)
    {
        if (await _dataContextInUse.Countries.AnyAsync()) return;

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

            if (await _dataContextInUse.Countries
                    .AnyAsync(c => c.Name == countryName))
                continue;

            var nationalityName = GetNationalityName(countryName);
            var country = new Country
            {
                Name = countryName,
                Cities = new List<City>(),
                WasDeleted = false,
                Nationality = null,
                IdGuid = Guid.NewGuid(),
                CreatedBy = createdBy,
                ProfilePhotoId = default,
            };
            var nationality = new Nationality
            {
                Name = nationalityName,
                IdGuid = Guid.NewGuid(),
                CreatedBy = createdBy,
                Country = country,
            };

            var cities = CreateCities(cityNames, createdBy, country);
            country.Cities = cities;
            country.Nationality = nationality;

            await _dataContextInUse.Countries.AddAsync(country);
        }
    }


    private List<City> CreateCities(
        IEnumerable<string> cityNames, User createdBy, Country country = null)
    {
        return cityNames.Select(
            cityName => new City
            {
                Name = cityName,
                WasDeleted = false,
                IdGuid = Guid.NewGuid(),
                CreatedBy = createdBy,
                ProfilePhotoId = default,
                CountryId = country?.Id ?? 0,
                Country = country ?? null
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