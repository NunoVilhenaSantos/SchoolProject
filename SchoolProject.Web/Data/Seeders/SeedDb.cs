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
using SchoolProject.Web.Data.EntitiesOthers;
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
        // DcMsSqlLocal msSqlLocal, DcMsSqlOnline msSqlOnline,
        // DCMySqlLocal mySqlLocal, DCMySqlOnline mySqlOnline
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
        // _msSqlOnline = msSqlOnline;
        // _mySqlLocal = mySqlLocal;
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
        SeedDbUsers.Initialize(userHelper: _userHelper, logger: _loggerSeedDbUsers);

        Console.WriteLine(format: "Seeding the database.", arg0: Color.Green);
        Console.WriteLine(format: _dataContextInUse.Database.GetConnectionString(),
            arg0: Color.Green);
        Console.WriteLine(
            format: _dataContextInUse.Database.GetDbConnection().DataSource,
            arg0: Color.Green);
        Console.WriteLine(format: "Debug point.", arg0: Color.Red);

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
            email: "nuno.santos.26288@formandos.cinel.pt");


        // ------------------------------------------------------------------ //
        // adding countries and cities to the database
        // ------------------------------------------------------------------ //
        Debug.Assert(condition: user != null, message: nameof(user) + " != null");
        await AddCountriesWithCitiesAndNationalities(createdBy: user);
        await _dataContextInUse.SaveChangesAsync();


        // ------------------------------------------------------------------ //
        // adding genres to the database
        // ------------------------------------------------------------------ //
        await SeedingDataGenders(user: user);
        await _dataContextInUse.SaveChangesAsync();


        // ------------------------------------------------------------------ //
        // adding students and teachers to the database and also there user
        // ------------------------------------------------------------------ //
        SeedDbStudentsAndTeachers.Initialize(userHelper: _userHelper, dataContext: _dataContextInUse);
        await SeedDbStudentsAndTeachers.AddingData(user: user);
        await _dataContextInUse.SaveChangesAsync();


        // ------------------------------------------------------------------ //
        // adding students and teachers to the database and also there user
        // ------------------------------------------------------------------ //
        SeedDbSchoolClasses.Initialize(dataContextInUse: _dataContextInUse);
        await SeedDbSchoolClasses.AddingData(user: user);
        await _dataContextInUse.SaveChangesAsync();


        // TODO: estou aqui a tentar adicionar os cursos aos professores
        // ------------------------------------------------------------------ //
        // adding courses to teachers to the database
        // ------------------------------------------------------------------ //
        // SeedDbTeachersWithCourses.Initialize(_dataContextInUse);
        await SeedDbTeachersWithCourses.AddingData(dataContextInUse: _dataContextInUse, user: user);
        await _dataContextInUse.SaveChangesAsync();

        Console.WriteLine(format: "Debug point.", arg0: Color.Red);


        // TODO: estou aqui a tentar adicionar os cursos as turmas e vice-versa
        // ------------------------------------------------------------------ //
        // adding courses to teachers to the database
        // ------------------------------------------------------------------ //
        // SeedDbSchoolClassesWithCourses.Initialize(_dataContextInUse);
        await SeedDbSchoolClassesWithCourses.AddingData(
            user: user, dataContextInUse: _dataContextInUse);
        await _dataContextInUse.SaveChangesAsync();

        Console.WriteLine(format: "Debug point.", arg0: Color.Red);


        // TODO: estou aqui a tentar adicionar os cursos as turmas e vice-versa
        // ------------------------------------------------------------------ //
        // adding students to school-classes into the database
        // ------------------------------------------------------------------ //
        // SeedDbStudentsWithSchoolClasses.Initialize(_dataContextInUse);
        await SeedDbStudentsWithSchoolClasses.AddingData(dataContextInUse: _dataContextInUse);
        await _dataContextInUse.SaveChangesAsync();

        Console.WriteLine(format: "Debug point.", arg0: Color.Red);


        // TODO: estou aqui a tentar adicionar o resto dos seedings que faltam
        // lista de schoolclasses no estudante e vice-versa


        // ------------------------------------------------------------------ //
        Console.WriteLine(format: "Seeding the database finished.", arg0: Color.Green);
        // ------------------------------------------------------------------ //


        // ------------------------------------------------------------------ //
        // verificar se existem os placeholders no sistema
        // ------------------------------------------------------------------ //
        SeedDbPlaceHolders.Initialize(hostingEnvironment: _hostingEnvironment);
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
                await Task.Delay(delay: TimeSpan.FromSeconds(value: 5));

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
                .Where(predicate: g => gendersToAdd.Contains(g.Name))
                .Select(selector: g => g.Name)
                .ToListAsync();

        var gendersToCreate =
            gendersToAdd.Except(second: existingGenders).ToList();

        var newGender =
            gendersToCreate.Select(selector: gender => new Gender
            {
                Name = gender,
                IdGuid = Guid.NewGuid(),
                WasDeleted = false,
                CreatedAt = DateTime.Now,
                CreatedBy = user
            }).ToList();

        await _dataContextInUse.Genders.AddRangeAsync(entities: newGender);
        await _dataContextInUse.SaveChangesAsync();
    }


    private async Task SeedingDataSuperUsers()
    {
        await SeedDbUsers.AddUsers(
            firstName: "Nuno", lastName: "Vilhena Santos",
            email: "nunovilhenasantos@msn.com",
            address: "Calle Luna", role: "SuperUser");

        await SeedDbUsers.AddUsers(
            firstName: "Nuno", lastName: "Santos",
            email: "nuno.santos.26288@formandos.cinel.pt",
            address: "Calle Luna", role: "SuperUser");

        await SeedDbUsers.AddUsers(
            firstName: "Rafael", lastName: "Santos",
            email: "rafael.santos@cinel.pt",
            address: "Calle Luna", role: "SuperUser");
    }


    private async Task SeedingDataAdminUsers()
    {
        await SeedDbUsers.AddUsers(
            firstName: "Jorge", lastName: "Pinto",
            email: "jorge.pinto.28720@formandos.cinel.pt",
            address: "Calle Luna", role: "Admin");

        await SeedDbUsers.AddUsers(
            firstName: "Ruben", lastName: "Correia",
            email: "ruben.corrreia.28257@formandos.cinel.pt",
            address: "Calle Luna", role: "Admin");

        await SeedDbUsers.AddUsers(
            firstName: "Tatiane", lastName: "Avellar",
            email: "tatiane.avellar.24718@formandos.cinel.pt",
            address: "Calle Luna", role: "Admin");
    }


    private async Task SeedingDataFunctionaryUsers()
    {
        await SeedDbUsers.AddUsers(
            firstName: "Licinio", lastName: "Rosario",
            email: "licinio.do.rosario@formandos.cinel.pt",
            address: "Calle Luna", role: "Functionary");

        await SeedDbUsers.AddUsers(
            firstName: "Joel", lastName: "Rangel",
            email: "joel.rangel.22101@formandos.cinel.pt",
            address: "Calle Luna", role: "Functionary");

        await SeedDbUsers.AddUsers(
            firstName: "Diogo", lastName: "Alves",
            email: "diogo.alves.28645@formandos.cinel.pt",
            address: "Calle Luna", role: "Functionary");
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
                .Where(predicate: role => rolesToCreate.Contains(role.Name))
                .ToListAsync();

        var rolesToAdd = rolesToCreate
            .Except(second: existingRoles.Select(selector: role => role.Name))
            .ToList();

        foreach (var role in rolesToAdd)
        {
            await CreateRoleAsync(role: role);

            // Associar a função com as reivindicações apropriadas
            switch (role)
            {
                case "SuperUser":
                    await AddClaimToRoleAsync(roleName: role, claimType: "IsAdmin");
                    await AddClaimToRoleAsync(roleName: role, claimType: "IsFunctionary");
                    await AddClaimToRoleAsync(roleName: role, claimType: "IsStudent");
                    await AddClaimToRoleAsync(roleName: role, claimType: "IsTeacher");
                    await AddClaimToRoleAsync(roleName: role, claimType: "IsParent");
                    await AddClaimToRoleAsync(roleName: role, claimType: "IsUser");

                    // ... outras reivindicações conforme necessário
                    break;

                case "Admin":
                    await AddClaimToRoleAsync(roleName: role, claimType: "IsAdmin");
                    // ...
                    break;

                case "Functionary":
                    await AddClaimToRoleAsync(roleName: role, claimType: "IsFunctionary");
                    // ...
                    break;

                case "Student":
                    await AddClaimToRoleAsync(roleName: role, claimType: "IsStudent");
                    // ...
                    break;

                case "Teacher":
                    await AddClaimToRoleAsync(roleName: role, claimType: "IsTeacher");
                    // ...
                    break;

                case "Parent":
                    await AddClaimToRoleAsync(roleName: role, claimType: "IsParent");
                    // ...
                    break;

                case "User":
                    await AddClaimToRoleAsync(roleName: role, claimType: "IsUser");
                    // ...
                    break;
            }
        }

        await _dataContextInUse.SaveChangesAsync();
    }


    private async Task CreateRoleAsync(string role)
    {
        await _roleManager.CreateAsync(role: new IdentityRole(roleName: role));
    }


    private async Task AddClaimToRoleAsync(string roleName, string claimType)
    {
        var role = await _roleManager.FindByNameAsync(roleName: roleName);

        if (role != null)
        {
            var claim = new Claim(type: claimType, value: "true");
            await _roleManager.AddClaimAsync(role: role, claim: claim);
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
                    .AnyAsync(predicate: c => c.Name == countryName))
                continue;

            var cities = CreateCities(cityNames: cityNames, createdBy: createdBy);
            var nationalityName = GetNationalityName(countryName: countryName);
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

            await _dataContextInUse.Countries.AddAsync(entity: country);
        }
    }


    private List<City> CreateCities(List<string> cityNames, User createdBy)
    {
        return cityNames.Select(
            selector: cityName => new City
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