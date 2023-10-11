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
using SchoolProject.Web.Data.EntitiesOthers;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Seeders;

/// <summary>
///     Data seeder for the database.
/// </summary>
public class SeedDb
{
    private readonly IConfiguration _configuration;

    //private readonly DataContextMsSql _dataContextInUse;
    private readonly DataContextMySql _dataContextInUse;
    //private readonly DataContextSqLite _dataContextInUse;

    private readonly DataContextMsSql _dataContextMsSql;
    // private readonly DcMsSqlLocal _msSqlLocal;
    // private readonly DcMsSqlOnline _msSqlOnline;

    private readonly DataContextMySql _dataContextMySql;
    // private readonly DcMySqlLocal _mySqlLocal;
    // private readonly DcMySqlOnline _mySqlOnline;

    private readonly DataContextSqLite _dataContextSqLite;


    private readonly IWebHostEnvironment _hostingEnvironment;

    private readonly ILogger<SeedDb> _logger;
    private readonly ILogger<SeedDbCourses> _loggerSeedDbCourses;
    private readonly ILogger<SeedDbStudentsAndTeachers> _loggerSeedDbSTs;
    private readonly ILogger<SeedDbUsers> _loggerSeedDbUsers;


    private readonly RoleManager<IdentityRole> _roleManager;


    private readonly IUserHelper _userHelper;
    private readonly UserManager<User> _userManager;


    /// <summary>
    ///
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="loggerSeedDbUsers"></param>
    /// <param name="loggerSeedDbSCs"></param>
    /// <param name="loggerSeedDbSTs"></param>
    /// <param name="userHelper"></param>
    /// <param name="userManager"></param>
    /// <param name="roleManager"></param>
    /// <param name="hostingEnvironment"></param>
    /// <param name="configuration"></param>
    /// <param name="dataContextMsSql"></param>
    /// <param name="dataContextMySql"></param>
    /// <param name="dataContextSqLite"></param>
    /// <param name="dataContextInUse"></param>
    public SeedDb(
        ILogger<SeedDb> logger,
        ILogger<SeedDbUsers> loggerSeedDbUsers,
        ILogger<SeedDbCourses> loggerSeedDbSCs,
        ILogger<SeedDbStudentsAndTeachers> loggerSeedDbSTs,
        IUserHelper userHelper,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        IWebHostEnvironment hostingEnvironment,
        IConfiguration configuration,
        DataContextSqLite dataContextSqLite,
        DataContextMySql dataContextInUse,
        DataContextMsSql dataContextMsSql,
        // DcMsSqlLocal msSqlLocal,
        // DcMySqlOnline mySqlOnline,
        DataContextMySql dataContextMySql
        // DcMySqlLocal mySqlLocal,
        // DcMsSqlOnline msSqlOnline
    )
    {
        _logger = logger;
        _loggerSeedDbCourses = loggerSeedDbSCs;
        _loggerSeedDbSTs = loggerSeedDbSTs;
        _loggerSeedDbUsers = loggerSeedDbUsers;

        _userHelper = userHelper;
        _userManager = userManager;
        _roleManager = roleManager;

        _hostingEnvironment = hostingEnvironment;
        _configuration = configuration;


        _dataContextInUse = dataContextInUse;


        _dataContextMsSql = dataContextMsSql;
        // _msSqlLocal = msSqlLocal;
        // _msSqlOnline = msSqlOnline;

        _dataContextMySql = dataContextMySql;
        // _mySqlLocal = mySqlLocal;
        // _mySqlOnline = mySqlOnline;

        _dataContextSqLite = dataContextSqLite;
    }


    /// <summary>
    /// Seeder assíncrono
    /// </summary>
    public async Task SeedAsync()
    {
        // ------------------------------------------------------------------ //
        // _dataContextInUse = _dataContextMySql;
        // ------------------------------------------------------------------ //

        // ------------------------------------------------------------------ //
        // verify if the database exists and if not create it
        // ------------------------------------------------------------------ //


        try
        {
            // TODO: tem bug sem dar erro no debug
            await _dataContextMySql.Database.MigrateAsync();
            // await _mySqlLocal.Database.MigrateAsync();
            // await _mySqlOnline.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            // Registe a exceção ou faça o tratamento adequado aqui.
            _logger.LogError(ex,
                "Ocorreu um erro durante a migração do banco de dados MySQL.");
            throw; // Re-lança a exceção para que o programa saiba que algo deu errado.
        }


        try
        {
            // TODO: tem bug sem dar erro no debug
            await _dataContextSqLite.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            // Registe a exceção ou faça o tratamento adequado aqui.
            _logger.LogError(ex,
                "Ocorreu um erro durante a migração do banco de dados SQLite.");
            throw; // Re-lança a exceção para que o programa saiba que algo deu errado.
        }


        try
        {
            // TODO: tem bug sem dar erro no debug
            await _dataContextMsSql.Database.MigrateAsync();
            // await _msSqlLocal.Database.MigrateAsync();
            // await _msSqlOnline.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            // Registe a exceção ou faça o tratamento adequado aqui.
            _logger.LogError(ex,
                "Ocorreu um erro durante a migração do banco de dados MsSQL.");
            throw; // Re-lança a exceção para que o programa saiba que algo deu errado.
        }


        try
        {
            // TODO: tem bug sem dar erro no debug
            await _dataContextInUse.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            // Registe a exceção ou faça o tratamento adequado aqui.
            _logger.LogError(ex,
                "Ocorreu um erro durante a migração do banco de dados MySQL.");
            throw; // Re-lança a exceção para que o programa saiba que algo deu errado.
        }


        // ------------------------------------------------------------------ //

        await _dataContextInUse.Database.MigrateAsync();
        await _dataContextInUse.Database.EnsureCreatedAsync();

        var value = _dataContextInUse.Database.GenerateCreateScript();

        // ------------------------------------------------------------------ //


        // ------------------------------------------------------------------ //
        // initialize SeedDbUsers with the user helper before been used
        // ------------------------------------------------------------------ //
        SeedDbUsers.Initialize(
            _userHelper, _loggerSeedDbUsers, _dataContextInUse);


        Console.WriteLine("Seeding the database.", Color.Green);

        Console.WriteLine(
            _dataContextInUse.Database.GetConnectionString() ?? string.Empty,
            Color.Green);

        Console.WriteLine(
            _dataContextInUse.Database.GetDbConnection().DataSource,
            Color.Green);

        Console.WriteLine("Debug point.", Color.Red);


        // ------------------------------------------------------------------ //
        // adding roles for all users in the system
        // ------------------------------------------------------------------ //
        await SeedingRolesForUsers();
        await _dataContextInUse.Database.CommitTransactionAsync();
        await _dataContextInUse.SaveChangesAsync();

        // Chame este método em vez de repetir o código 15 vezes.
        await CommitChangesAndHandleErrorsAsync();


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
        SeedDbCourses.Initialize(_dataContextInUse);
        await SeedDbCourses.AddingData(user);
        await _dataContextInUse.SaveChangesAsync();


        // ------------------------------------------------------------------ //
        // adding courses to teachers to the database
        // ------------------------------------------------------------------ //
        // SeedDbTeachersWithDisciplines.Initialize(_dataContextInUse);
        await SeedDbTeachersWithDisciplines.AddingData(_dataContextInUse, user);
        await _dataContextInUse.SaveChangesAsync();

        Console.WriteLine("Debug point.", Color.Red);


        // ------------------------------------------------------------------ //
        // adding courses to teachers to the database
        // ------------------------------------------------------------------ //
        // SeedDbCoursesWithDisciplines.Initialize(_dataContextInUse);
        await SeedDbCoursesWithDisciplines.AddingData(user, _dataContextInUse);
        await _dataContextInUse.SaveChangesAsync();

        Console.WriteLine("Debug point.", Color.Red);


        // ------------------------------------------------------------------ //
        // adding students to school-classes into the database
        // ------------------------------------------------------------------ //
        // SeedDbStudentsAndCourses.Initialize(_dataContextInUse);
        await SeedDbStudentsAndCourses.AddingData(user, _dataContextInUse);
        await _dataContextInUse.SaveChangesAsync();

        Console.WriteLine("Debug point.", Color.Red);


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


        // ------------------------------------------------------------------ //
        SaveToCsv.SaveTo(_dataContextInUse);
        // ------------------------------------------------------------------ //



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
        var rolesToCreate = new System.Collections.Generic.List<string>
        {
            "SuperUser", "Admin", "Functionary",
            "Student", "Teacher", "Parent",
            "User"
        };

        //var existingRoles =
        //    await _roleManager.Roles
        //        .Where(role => rolesToCreate.Contains(role.Name))
        //        .ToListAsync();

        var existingRoles =
            _roleManager.Roles
                .Where(role => rolesToCreate.Contains(role.Name))
                .ToList();

        var rolesToAdd = rolesToCreate
            .Except(existingRoles.Select(role => role.Name))
            .ToList();

        foreach (var role in rolesToAdd)
        {
            //CreateRole(role).Wait();

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
                    //AddClaimToRole(role, "IsAdmin").Wait();
                    await AddClaimToRoleAsync(role, "IsAdmin");
                    break;

                case "Functionary":
                    //AddClaimToRoleAsync(role, "IsFunctionary").Wait();
                    await AddClaimToRoleAsync(role, "IsFunctionary");
                    break;

                case "Student":
                    await AddClaimToRoleAsync(role, "IsStudent");
                    break;

                case "Teacher":
                    await AddClaimToRoleAsync(role, "IsTeacher");
                    break;

                case "Parent":
                    await AddClaimToRoleAsync(role, "IsParent");
                    break;

                case "User":
                    await AddClaimToRoleAsync(role, "IsUser");
                    break;
            }
        }

        await _dataContextInUse.SaveChangesAsync();
    }


    private Task CreateRole(string role) =>
        _roleManager.CreateAsync(new IdentityRole(role));

    private async Task CreateRoleAsync(string role) =>
        await _roleManager.CreateAsync(new IdentityRole(role));


    private Task AddClaimToRole(string roleName, string claimType)
    {
        var role = _roleManager.FindByNameAsync(roleName).Result;

        if (role != null)
        {
            var claim = new Claim(claimType, "true");
            _roleManager.AddClaimAsync(role, claim);
        }

        return Task.CompletedTask;
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
                Cities = new HashSet<City>(),
                WasDeleted = false,
                Nationality = null,
                IdGuid = Guid.NewGuid(),
                CreatedBy = createdBy,
                ProfilePhotoId = default
            };
            var nationality = new Nationality
            {
                Name = nationalityName,
                IdGuid = Guid.NewGuid(),
                CreatedBy = createdBy,
                Country = country
                //CountryId = country.Id
            };

            var cities = CreateCities(cityNames, createdBy, country);
            country.Cities = cities;
            country.Nationality = nationality;

            await _dataContextInUse.Countries.AddAsync(country);
        }
    }


    private HashSet<City> CreateCities(
        IEnumerable<string> cityNames, User createdBy, Country country)
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
                Country = country ?? _dataContextSqLite.Countries
                    .FirstOrDefault(c => c.Name == "Portugal")
            }
        ).ToHashSet();
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


    /// <summary>
    /// Seeder síncrono
    /// </summary>
    public Task SeedSync()
    {
        // ------------------------------------------------------------------ //
        // _dataContextInUse = _dataContextMySql;
        // ------------------------------------------------------------------ //

        // ------------------------------------------------------------------ //
        // verify if the database exists and if not create it
        // ------------------------------------------------------------------ //

        Console.WriteLine("Seeding the database.", Color.Green);

        try
        {
            // TODO: tem bug sem dar erro no debug
            //_dataContextMySql.Database.OpenConnection();
            // _dataContextMySql.Database.EnsureDeleted();
            // _dataContextMySql.Database.Migrate();
            _dataContextMySql.Database.EnsureCreated();

            // await _mySqlLocal.Database.MigrateAsync();
            // await _mySqlOnline.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            // Registe a exceção ou faça o tratamento adequado aqui.
            _logger.LogError(ex,
                "Ocorreu um erro durante a migração do banco de dados MySQL.");

            // Re-lança a exceção para que o programa saiba que algo deu errado.
            throw; 
        }


        try
        {
            // TODO: tem bug sem dar erro no debug
            //_dataContextSqLite.Database.OpenConnection();
            //_dataContextSqLite.Database.EnsureDeleted();
            //_dataContextSqLite.Database.Migrate();
            _dataContextSqLite.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            // Registe a exceção ou faça o tratamento adequado aqui.
            _logger.LogError(ex,
                "Ocorreu um erro durante a migração do banco de dados SQLite.");
            throw; // Re-lança a exceção para que o programa saiba que algo deu errado.
        }


        try
        {
            // TODO: tem bug sem dar erro no debug
            // await _msSqlLocal.Database.MigrateAsync();
            // await _msSqlOnline.Database.MigrateAsync();
            //_dataContextMsSql.Database.OpenConnection();
            //_dataContextMsSql.Database.EnsureDeleted();
            //_dataContextMsSql.Database.Migrate();
            _dataContextMsSql.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            // Registe a exceção ou faça o tratamento adequado aqui.
            _logger.LogError(ex,
                "Ocorreu um erro durante a migração do banco de dados MsSQL.");
            throw; // Re-lança a exceção para que o programa saiba que algo deu errado.
        }


        try
        {
            // TODO: tem bug sem dar erro no debug
            //await _dataContextInUse.Database.MigrateAsync();
            //_dataContextInUse.Database.OpenConnection();
            // Isso apagará todas as tabelas e dados existentes
            //_dataContextInUse.Database.EnsureDeleted();
            //_dataContextInUse.Database.Migrate();
            _dataContextInUse.Database.EnsureCreated();
        }
        catch (Exception ex)
        {
            // Registe a exceção ou faça o tratamento adequado aqui.
            _logger.LogError(ex,
                "Ocorreu um erro durante a migração do banco de dados MySQL.");
            throw; // Re-lança a exceção para que o programa saiba que algo deu errado.
        }


        // ------------------------------------------------------------------ //


        var value = _dataContextInUse.Database.GenerateCreateScript();

        // ------------------------------------------------------------------ //


        // ------------------------------------------------------------------ //
        // initialize SeedDbUsers with the user helper before been used
        // ------------------------------------------------------------------ //
        SeedDbUsers.Initialize(
            _userHelper, _loggerSeedDbUsers, _dataContextInUse);


        Console.WriteLine("Seeding the database.", Color.Green);

        Console.WriteLine(
            _dataContextInUse.Database.GetConnectionString() ?? string.Empty,
            Color.Green);

        Console.WriteLine(
            _dataContextInUse.Database.GetDbConnection().DataSource,
            Color.Green);

        Console.WriteLine("Debug point.", Color.Red);


        // ------------------------------------------------------------------ //
        // adding roles for all users in the system
        // ------------------------------------------------------------------ //
        SeedingRolesForUsers().Wait();

        // _dataContextInUse.SaveChanges();
        CommitChangesAndHandleErrors();

        // _dataContextInUse.Database.CommitTransaction();
        // _dataContextInUse.Database.CommitTransactionAsync();


        // ------------------------------------------------------------------ //
        // adding the super users
        // ------------------------------------------------------------------ //
        SeedingDataSuperUsers().Wait();
        // _dataContextInUse.SaveChanges();
        CommitChangesAndHandleErrors();


        // ------------------------------------------------------------------ //
        // adding the admin user
        // ------------------------------------------------------------------ //
        SeedingDataAdminUsers().Wait();
        // _dataContextInUse.SaveChanges();
        CommitChangesAndHandleErrors();


        // ------------------------------------------------------------------ //
        // adding the Functionary user
        // ------------------------------------------------------------------ //
        SeedingDataFunctionaryUsers().Wait();
        // _dataContextInUse.SaveChanges();
        CommitChangesAndHandleErrors();


        // ------------------------------------------------------------------ //
        // getting an admin to insert data that needs to have an user
        // ------------------------------------------------------------------ //
        var user = _userHelper.GetUserByEmailAsync(
            "nuno.santos.26288@formandos.cinel.pt").Result;


        // ------------------------------------------------------------------ //
        // adding countries and cities to the database
        // ------------------------------------------------------------------ //
        Debug.Assert(user != null, nameof(user) + " != null");

        AddCountriesWithCitiesAndNationalities(user).Wait();


        // _dataContextInUse.SaveChanges();
        CommitChangesAndHandleErrors();


        // ------------------------------------------------------------------ //
        // adding genres to the database
        // ------------------------------------------------------------------ //
        SeedingDataGenders(user).Wait();

        // _dataContextInUse.SaveChanges();
        CommitChangesAndHandleErrors();


        // ------------------------------------------------------------------ //
        // adding students and teachers to the database and also there user
        // ------------------------------------------------------------------ //
        SeedDbStudentsAndTeachers.Initialize(_userHelper, _dataContextInUse);
        SeedDbStudentsAndTeachers.AddingData(user).Wait();

        // _dataContextInUse.SaveChanges();
        CommitChangesAndHandleErrors();


        // ------------------------------------------------------------------ //
        // adding students and teachers to the database and also there user
        // ------------------------------------------------------------------ //
        SeedDbCourses.Initialize(_dataContextInUse);
        SeedDbCourses.AddingData(user).Wait();

        // _dataContextInUse.SaveChanges();
        CommitChangesAndHandleErrors();


        // ------------------------------------------------------------------ //
        // adding courses to teachers to the database
        // ------------------------------------------------------------------ //
        // SeedDbTeachersWithDisciplines.Initialize(_dataContextInUse);
        SeedDbTeachersWithDisciplines.AddingData(_dataContextInUse, user)
            .Wait();
        _dataContextInUse.SaveChanges();

        Console.WriteLine("Debug point.", Color.Red);


        // ------------------------------------------------------------------ //
        // adding courses to teachers to the database
        // ------------------------------------------------------------------ //
        // SeedDbCoursesWithDisciplines.Initialize(_dataContextInUse);
        SeedDbCoursesWithDisciplines.AddingData(user, _dataContextInUse).Wait();

        // _dataContextInUse.SaveChanges();
        CommitChangesAndHandleErrors();

        Console.WriteLine("Debug point.", Color.Red);


        // ------------------------------------------------------------------ //
        // adding students to school-classes into the database
        // ------------------------------------------------------------------ //
        // SeedDbStudentsAndCourses.Initialize(_dataContextInUse);
        SeedDbStudentsAndCourses.AddingData(user, _dataContextInUse).Wait();

        // _dataContextInUse.SaveChanges();
        CommitChangesAndHandleErrors();

        Console.WriteLine("Debug point.", Color.Red);


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
        _dataContextInUse.SaveChanges();



        // ------------------------------------------------------------------ //
        SaveToCsv.SaveTo(_dataContextInUse);



        // ------------------------------------------------------------------ //
        // ------------------------------------------------------------------ //
        try
        {
            // Realiza as transações necessárias e
            // salva as alterações no banco de dados
            _dataContextInUse.SaveChanges();

            // Enquanto houver uma transação ativa,
            // aguarde e verifique novamente
            while (_dataContextInUse.Database.GetEnlistedTransaction() != null)
                // Aguardar um período de tempo
                // Exemplo: 5 segundos
                Task.Delay(TimeSpan.FromSeconds(5));

            // Todas as transações estão concluídas,
            // então chama Dispose para liberar o contexto
            _dataContextInUse.Dispose();
        }
        catch (Exception ex)
        {
            // Lida com exceções, se necessário
        }
        finally
        {
            // Se você não quiser mais que o contexto seja reutilizado,
            // chame Dispose novamente
            _dataContextInUse.Dispose();
        }

        return Task.FromResult(true);
    }


    private Task<AppResponse> CommitChangesAndHandleErrors()
    {

        // Commit the changes to the database and use asynchronous commit
        if (_dataContextInUse.Database.CurrentTransaction != null)
        {
            // Há uma transação ativa, você pode efetuar o commit.
            _dataContextInUse.Database.CommitTransaction();
        }


        try
        {
            // Save any changes made to the database context
            _dataContextInUse.SaveChanges();

            // Operações bem-sucedidas, retorna um objeto de sucesso
            return Task.FromResult(new AppResponse
            {
                IsSuccess = true,
                Message = "Operações de commit concluídas com sucesso."
            });
        }
        catch (Exception ex)
        {
            // Handle any exceptions that might occur
            _logger.LogError(ex,
                "Error during database seeding and transaction commit.");

            // Consider appropriate error handling and
            // possibly rolling back the transaction

            // Retorna um objeto de falha com a mensagem de erro
            return Task.FromResult(new AppResponse
            {
                IsSuccess = false,
                Message = "Erro durante o commit do " +
                          "banco de dados e transação: " +
                          ex.Message
            });
        }
    }


    private async Task CommitChangesAndHandleErrorsAsync()
    {
        try
        {
            // Commit the changes to the database and use asynchronous commit
            await _dataContextInUse.Database.CommitTransactionAsync();

            // Save any changes made to the database context
            await _dataContextInUse.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Handle any exceptions that might occur
            _logger.LogError(ex,
                "Error during database seeding and transaction commit.");

            // Consider appropriate error handling and possibly rolling back the transaction

            throw; // Re-throw the exception to indicate a failure in the seeding process
        }
    }

    internal void CloseConnections()
    {
        _dataContextInUse.Database.CloseConnection();
        _dataContextMsSql.Database.CloseConnection();
        _dataContextMySql.Database.CloseConnection();
        _dataContextSqLite.Database.CloseConnection();
        // _dataContextInUse.Database.CloseConnection();
    }
}