using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities;
using SchoolProject.Web.Helpers;

namespace SchoolProject.Web.Data.Seeders;

public class SeedDb
{
    private readonly DataContextMssql _dataContextMssql;
    private readonly DataContextMySql _dataContextMySql;
    private readonly DataContextSqLite _dataContextSqLite;


    // private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly IUserHelper _userHelper;


    public SeedDb(
        IUserHelper userHelper,
        DataContextMssql dataContextMssql,
        DataContextMySql dataContextMySql,
        DataContextSqLite dataContextSqLite

        // UserManager<User> userManager,
        // RoleManager<IdentityRole> roleManager
    )
    {
        _userHelper = userHelper;

        _dataContextMssql = dataContextMssql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;

        // _userManager = userManager;
        // _roleManager = roleManager;
    }


    public async Task SeedAsync()
    {
        // ------------------------------------------------------------------ //
        //
        // adding the Functionary user
        //
        // ------------------------------------------------------------------ //
        await _dataContextMssql.Database.MigrateAsync();
        await _dataContextMySql.Database.MigrateAsync();
        await _dataContextSqLite.Database.MigrateAsync();


        // ------------------------------------------------------------------ //
        //
        // adding the super-admin user
        //
        // ------------------------------------------------------------------ //
        await SeedDbUsers.AddUsers(
            "nuno.santos.26288@formandos.cinel.pt",
            "Calle Luna", "SuperUser", "Passw0rd");
        await SeedDbUsers.AddUsers(
            "rafael.santos@cinel.pt",
            "Calle Luna", "SuperUser", "Passw0rd");


        // ------------------------------------------------------------------ //
        //
        // adding the admin user
        //
        // ------------------------------------------------------------------ //
        await SeedDbUsers.AddUsers(
            "jorge.pinto.28720@formandos.cinel.pt",
            "Calle Luna", "Admin");

        await SeedDbUsers.AddUsers(
            "ruben.corrreia.28257@formandos.cinel.pt",
            "Calle Luna", "Admin");

        await SeedDbUsers.AddUsers(
            "tatiane.avellar.24718@formandos.cinel.pt",
            "Calle Luna", "Admin", "Passw0rd");


        // ------------------------------------------------------------------ //
        //
        // adding the Functionary user
        //
        // ------------------------------------------------------------------ //
        await SeedDbUsers.AddUsers(
            "licinio.do.rosario@formandos.cinel.pt",
            "Calle Luna", "Functionary");

        await SeedDbUsers.AddUsers(
            "joel.rangel.22101@formandos.cinel.pt",
            "Calle Luna", "Functionary");

        await SeedDbUsers.AddUsers(
            "diogo.alves.28645@formandos.cinel.pt",
            "Calle Luna", "Functionary", "Passw0rd");


        // ------------------------------------------------------------------ //
        //
        // adding physical persons to the database and also is user
        //
        // ------------------------------------------------------------------ //
        await SeedDbPersons.AddingData();


        // verificar se existem os placeholders no sistema
        SeedDbPlaceHolders.AddPlaceHolders();


        await _dataContextMssql.SaveChangesAsync();
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