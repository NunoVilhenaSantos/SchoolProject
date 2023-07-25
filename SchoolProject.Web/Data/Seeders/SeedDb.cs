using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.ExtraTables;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.Users;

namespace SchoolProject.Web.Data.Seeders;

public class SeedDb
{
    private readonly DataContextMsSql _dataContextMsSql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextSqLite _dataContextSqLite;
    private readonly RoleManager<IdentityRole> _roleManager;


    private readonly IUserHelper _userHelper;
    private readonly UserManager<User> _userManager;


    public SeedDb(
        IUserHelper userHelper,
        UserManager<User> userManager,
        RoleManager<IdentityRole> roleManager,
        DataContextMsSql dataContextMsSql,
        DataContextMySql dataContextMySql,
        DataContextSqLite dataContextSqLite
    )
    {
        _userHelper = userHelper;
        _userManager = userManager;
        _roleManager = roleManager;

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
        // adding the Functionary user
        // ------------------------------------------------------------------ //
        var user = await _userHelper.GetUserByEmailAsync(
            "nuno.santos.26288@formandos.cinel.pt");


        // ------------------------------------------------------------------ //
        // adding genres to the database
        // ------------------------------------------------------------------ //
        await SeedingDataGenres(user);


        // ------------------------------------------------------------------ //
        // adding students and teachers to the database and also there user
        // ------------------------------------------------------------------ //
        await SeedDbPersons.AddingData();


        // verificar se existem os placeholders no sistema
        SeedDbPlaceHolders.AddPlaceHolders();


        await _dataContextMsSql.SaveChangesAsync();
    }


    private async Task SeedingDataGenres(User user)
    {
        var genres = new List<Genre>
        {
            new()
            {
                Name = "Male",
                IdGuid = new Guid(),
                WasDeleted = false,
                CreatedAt = DateTime.Now,
                CreatedBy = user
            },
            new()
            {
                Name = "Female",
                IdGuid = default,
                WasDeleted = false,
                CreatedAt = DateTime.Now,
                CreatedBy = user
            },
            new()
            {
                Name = "NonBinary",
                IdGuid = default,
                WasDeleted = false,
                CreatedAt = DateTime.Now,
                CreatedBy = user
            },
            new()
            {
                Name = "PreferNotToSay",
                IdGuid = default,
                WasDeleted = false,
                CreatedAt = DateTime.Now,
                CreatedBy = user
            }
        };
        await _dataContextMsSql.Genres.AddRangeAsync(genres);
    }


    private static async Task SeedingDataSuperUsers()
    {
        await SeedDbUsers.AddUsers(
            "Nuno", "Vilhena Santos",
            "nunovilhenasantos@msn.com",
            "Calle Luna", "SuperUser", "Passw0rd");
        await SeedDbUsers.AddUsers(
            "Nuno", "Santos",
            "nuno.santos.26288@formandos.cinel.pt",
            "Calle Luna", "SuperUser", "Passw0rd");
        await SeedDbUsers.AddUsers(
            "Rafael", "Santos",
            "rafael.santos@cinel.pt",
            "Calle Luna", "SuperUser", "Passw0rd");
    }


    private static async Task SeedingDataAdminUsers()
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
            "Calle Luna", "Admin", "Passw0rd");
    }


    private static async Task SeedingDataFunctionaryUsers()
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
            "Calle Luna", "Functionary", "Passw0rd");
    }


    private async Task<IdentityResult?> CheckRolesAsync(User user)
    {
        return await _roleManager.FindByIdAsync(user.Id) switch
        {
            null => await CreateRoleAsync(user.UserName),
            _ => null
        };
    }


    private async Task<IdentityResult> CreateRoleAsync(string? role)
    {
        // return await _roleManager.CreateAsync(new IdentityRole("Admin"));
        return await _roleManager.CreateAsync(new IdentityRole(role));
    }
}