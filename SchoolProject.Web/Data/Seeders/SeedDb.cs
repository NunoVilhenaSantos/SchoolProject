using Microsoft.AspNetCore.Identity;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities;
using SchoolProject.Web.Helpers;

namespace SchoolProject.Web.Data.Seeders;

public class SeedDb
{




    public const string MyLeasingAdminsNuno =
        "nuno.santos.26288@formandos.cinel.pt";

    public const string MyLeasingAdminsDiogo =
        "diogo.alves.28645@formandos.cinel.pt";

    public const string MyLeasingAdminsRuben =
        "ruben.corrreia.28257@formandos.cinel.pt";

    public const string MyLeasingAdminsTatiane =
        "tatiane.avellar.24718@formandos.cinel.pt";

    public const string MyLeasingAdminsJorge =
        "jorge.pinto.28720@formandos.cinel.pt";

    public const string MyLeasingAdminsJoel =
        "joel.rangel.22101@formandos.cinel.pt";

    public const string MyLeasingAdminsLicinio =
        "licinio.do.rosario@formandos.cinel.pt";


    private readonly DataContextMSSQL _dataContextMssql;
    private readonly DataContextMySQL _dataContextMySql;
    private readonly DataContextSQLite _dataContextSqLite;


    // Injeção de dependência do IWebHostEnvironment
    private readonly IWebHostEnvironment _hostingEnvironment;
    private readonly Random _random = new();

    // private readonly UserManager<User> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    private readonly IUserHelper _userHelper;

    public string PlaceHolders;




    public SeedDb(
        IUserHelper userHelper,

        DataContextMSSQL dataContextMssql,
        DataContextMySQL dataContextMySql,
        DataContextSQLite dataContextSqLite,
      
        IWebHostEnvironment hostingEnvironment

        // UserManager<User> userManager,
        // RoleManager<IdentityRole> roleManager

    )
    {
        _userHelper = userHelper;

        _dataContextMssql = dataContextMssql;
        _dataContextMySql = dataContextMySql;
        _dataContextSqLite = dataContextSqLite;

        _hostingEnvironment = hostingEnvironment;
        // _userManager = userManager;
        // _roleManager = roleManager;

    }


    public async Task SeedAsync()
    {
        await _dataContextMssql.Database.EnsureCreatedAsync();
        await _dataContextMySql.Database.EnsureCreatedAsync();
        await _dataContextSqLite.Database.EnsureCreatedAsync();


        await AddUsers(MyLeasingAdminsJorge,
            "Calle Luna", "Admin", "123456");
        await AddUsers(MyLeasingAdminsRuben,
            "Calle Luna", "Admin", "123456");
        await AddUsers(MyLeasingAdminsTatiane,
            "Calle Luna", "Admin", "123456");
        await AddUsers(MyLeasingAdminsJoel,
            "Calle Luna", "Admin", "123456");
        await AddUsers(MyLeasingAdminsLicinio,
            "Calle Luna", "Admin", "123456");
        await AddUsers(MyLeasingAdminsDiogo,
            "Calle Luna", "Admin", "123456");
        await AddUsers(MyLeasingAdminsNuno,
            "Calle Luna", "Admin", "123456");


        var user = await CheckUserAsync(
            "Juan", "Zuluaga",
            "admin@disto_tudo_e_que_rouba_a_descarada.com",
            "admin@disto_tudo_e_que_rouba_a_descarada.com",
            "111222333", "Admin",
            "document's", "Calle Luna"
        );

        // await CheckRolesAsync(user);

        if (!_dataContextMssql.Students.Any())
        {
            await AddStudent(
                "Juan", "Zuluaga", "Calle Luna", user);
            await AddStudent(
                "Joaquim", "Alvenaria", "Calle Sol", user);
            await AddStudent(
                "Alberto", "Domingues", "Calle Luna", user);
            await AddStudent(
                "Mariana", "Alvarez", "Calle Sol", user);
            await AddStudent(
                "Lucia", "Liu", "Calle Luna", user);
            await AddStudent(
                "Renee", "Arriaga", "Calle Sol", user);
            await AddStudent(
                "Marcia", "Zuluaga", "Calle Luna", user);
            await AddStudent(
                "Ernesto", "Guevara", "Calle Sol", user);
            await AddStudent(
                "El", "Che", "Calle Luna", user);
            await AddStudent(
                "Claudia", "Arroz", "Calle Sol", user);
        }


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

        // if (!_dataContext.Properties.Any())
        // {
        //     await AddProperties(user);
        // }

        // verificar se existem os placeholders no sistema
        AddPlaceHolders();


        await _dataContextMssql.SaveChangesAsync();
    }

    private void AddPlaceHolders()
    {
        var baseDirectory = _hostingEnvironment.ContentRootPath;
        var diretorioBase =
            Path.GetFullPath(Path.Combine(baseDirectory, "Helpers/Images"));

        var origem = Path.Combine(_hostingEnvironment.ContentRootPath,
            "Helpers", "Images");
        var destino = Path.Combine(_hostingEnvironment.WebRootPath,
            "images", "PlaceHolders");
        PlaceHolders = Path.Combine(_hostingEnvironment.WebRootPath,
            "images", "PlaceHolders");


        // Cria o diretório se não existir
        var folderPath = Path.Combine(
            Directory.GetCurrentDirectory(), "wwwroot", "images",
            "PlaceHolders");
        Directory.CreateDirectory(destino);
        Directory.Exists(destino);


        // Obtém todos os caminhos dos arquivos na pasta de origem
        var arquivos = Directory.GetFiles(origem);


        // Itera sobre os caminhos dos arquivos e copia cada um para a pasta de destino
        foreach (var arquivo in arquivos)
        {
            var nomeArquivo = Path.GetFileName(arquivo);
            var caminhoDestino = Path.Combine(destino, nomeArquivo);
            File.Copy(arquivo, caminhoDestino, true);
        }

        Console.WriteLine("Placeholders adicionados com sucesso!");
    }

    private async Task AddUsers(
        string email, string address,
        string role, string password
    )
    {
        var userSplit = email.Split(
            '.', StringSplitOptions.RemoveEmptyEntries);

        var document = _random.Next(100000, 999999999).ToString();
        var fixedPhone = _random.Next(1000000, 99999999).ToString();
        var cellPhone = _random.Next(1000000, 99999999).ToString();
        var addressFull = address + ", " + _random.Next(1, 9999);

        await CheckUserAsync(
            userSplit[0], userSplit[1],
            email,
            email,
            cellPhone, role,
            document,
            addressFull, password
        );
    }


    private async Task<User> CheckUserAsync(
        string firstName, string lastName,
        string userName,
        string email,
        string phoneNumber, string role,
        string document, string address,
        string password = "123456")
    {
        var user = await _userHelper.GetUserByEmailAsync(email);

        switch (user)
        {
            case null:
            {
                user = role switch
                {
                    "Student" => new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Address = address,
                        UserName = userName,
                        Email = email,
                        PhoneNumber = phoneNumber
                    },
                    "Teacher" => new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Address = address,
                        UserName = userName,
                        Email = email,
                        PhoneNumber = phoneNumber
                    },
                    "Functionary" => new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Address = address,
                        UserName = userName,
                        Email = email,
                        PhoneNumber = phoneNumber
                    },
                    "SuperUser" => new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Address = address,
                        UserName = userName,
                        Email = email,
                        PhoneNumber = phoneNumber
                    },
                    "Admin" => new User
                    {
                        FirstName = firstName,
                        LastName = lastName,
                        Address = address,
                        UserName = userName,
                        Email = email,
                        PhoneNumber = phoneNumber
                    },
                    _ => throw new InvalidOperationException(
                        "The role is not valid")
                };

                var result = await _userHelper.AddUserAsync(user, password);

                if (result != IdentityResult.Success)
                    throw new InvalidOperationException(
                        "Could not create the user in Seeder");
                break;
            }
        }

        return user;
    }


    private async Task<IdentityResult?> CheckRolesAsync(User user)
    {
        return await _roleManager.FindByIdAsync(user.Id) switch
        {
            null => await CreateRoleAsync(user.UserName),
            _ =>  null
            
        };
    }


    private async Task<IdentityResult> CreateRoleAsync(string role)
    {
        // return await _roleManager.CreateAsync(new IdentityRole("Admin"));
        return await _roleManager.CreateAsync(new IdentityRole(role));
    }


    private async Task AddStudent(
        string firstName, string lastName, string address, User user)
    {
        var document = _random.Next(100000, 999999999).ToString();
        var fixedPhone = _random.Next(1000000, 99999999).ToString();
        var cellPhone = _random.Next(1000000, 99999999).ToString();
        var addressFull = address + ", " + _random.Next(1, 9999);

        _dataContextMssql.Students.Add(new Student
        {
            //Document = document,
            FirstName = firstName,
            LastName = lastName,
            //FixedPhone = fixedPhone,
            //CellPhone = cellPhone,
            Address = addressFull,
            User = await CheckUserAsync(
                    firstName, lastName,
                    $"{firstName}.{lastName}@rouba_a_descarada.com",
                    $"{firstName}.{lastName}@rouba_a_descarada.com",
                    $"{cellPhone}", "Owner",
                    document, addressFull
                )
        }
        );

        // await _dataContextMssql.SaveChangesAsync();

    }


    private async Task AddTeacher(
        string firstName, string lastName, string address, User user)
    {
        var document = _random.Next(100000, 999999999).ToString();
        var fixedPhone = _random.Next(1000000, 99999999).ToString();
        var cellPhone = _random.Next(1000000, 99999999).ToString();
        var addressFull = address + ", " + _random.Next(1, 9999);

        _dataContextMssql.Teachers.Add(new Teacher
            {
                // Document = document,
                FirstName = firstName,
                LastName = lastName,
                // FixedPhone = fixedPhone,
                // CellPhone = cellPhone,
                Address = addressFull,
                User = await CheckUserAsync(
                    firstName, lastName,
                    $"{firstName}.{lastName}@rouba_a_descarada.com",
                    $"{firstName}.{lastName}@rouba_a_descarada.com",
                    $"{cellPhone}", "Lessee",
                    document, addressFull
                )
            }
        );

        // await _dataContextMssql.SaveChangesAsync();
    }


}