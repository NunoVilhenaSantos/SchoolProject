using System.Diagnostics;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Azure;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Tokens;
using SchoolProject.Web;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.DataContexts.MSSQL;
using SchoolProject.Web.Data.DataContexts.MySQL;
using SchoolProject.Web.Data.EntitiesOthers;
using SchoolProject.Web.Data.Seeders;
using SchoolProject.Web.Helpers;
using SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;
using SchoolProject.Web.Helpers.Email;
using SchoolProject.Web.Helpers.Images;
using SchoolProject.Web.Helpers.Services;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;
using Serilog;


// Create a new web application using the WebApplicationBuilder.
var builder = WebApplication.CreateBuilder(args: args);


// Helper method to generate a random
// string with specified length and characters.
static string GenerateRandomString(
    int length = 64, bool withSpecialCharacters = true)
{
    var characterSet =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    if (withSpecialCharacters) characterSet += "!@#$%^&*()_-+=[]{}|;:,.<>?";

    var random = new Random();
    var sb = new StringBuilder(capacity: length);

    for (var i = 0; i < length; i++)
    {
        var index = random.Next(maxValue: characterSet.Length);
        var randomCharacter = characterSet[index: index];
        sb.Append(value: randomCharacter);
    }

    var randomString = sb.ToString();
    // Print the generated string to the console
    Console.WriteLine(value: randomString);
    return randomString;
}


// Método para extrair o nome do servidor da Connection String
static string GetServerHostNameFromConnectionString(string connectionString)
{
    var parts = connectionString.Split(separator: ';');

    foreach (var part in parts)
        if (part.StartsWith(value: "Data Source="))
            return part["Data Source=".Length..];

        else if (part.StartsWith(value: "workstation id="))
            return part["workstation id=".Length..];

        else if (part.StartsWith(value: "Server="))
            return part["Server=".Length..];

        else if (part.StartsWith(value: "Host="))
            return part["Host=".Length..];


    throw new ArgumentException(
        message: "Connection String inválida. Nome do servidor não encontrado.");
}


// Método para extrair o nome do servidor da Connection String
static void GetServerHostNamePing(string serverHostName)
{
    // Tempo limite para o ping em milissegundos (3 segundo neste exemplo)
    var timeout = 3000;

    while (true)
    {
        var pingSender = new Ping();
        var reply = pingSender.Send(hostNameOrAddress: serverHostName, timeout: timeout);

        if (reply.Status == IPStatus.Success)
        {
            Console.WriteLine(value: "Servidor responde. Conexão estabelecida.");

            // Sai do loop quando a conexão é bem-sucedida
            break;
        }

        Console.WriteLine(
            value: "Falha na conexão com o servidor. Tentar novamente...");

        // Aguarda um período antes de tentar novamente
        // Aguarda 3 segundos (ajuste conforme necessário)
        // antes de tentar novamente, use 0 para tentar imediatamente
        // ou altere para o valor desejado em milissegundos
        // na variavel timeout
        Thread.Sleep(millisecondsTimeout: timeout);
    }
}


// Método para executar o seeding do banco de dados
static async Task RunSeeding(IHost host)
{
    // Create a new timer with the name "MyTimer"
    TimeTracker.CreateTimer(timerName: TimeTracker.SeederTimerName);

    // Start the timer "MyTimer"
    TimeTracker.StartTimer(timerName: TimeTracker.SeederTimerName);


    var scopeFactory = host.Services.GetService<IServiceScopeFactory>();

    using var scope = scopeFactory?.CreateScope();

    var seeder = scope?.ServiceProvider.GetService<SeedDb>();

    await seeder?.SeedAsync();


    // Stop the timer "MyTimer"
    TimeTracker.StopTimer(timerName: TimeTracker.SeederTimerName);

    // Get the elapsed time for the timer "MyTimer"
    var elapsed = TimeTracker.GetElapsedTime(timerName: TimeTracker.SeederTimerName);

    Console.WriteLine(value: $"Tempo decorrido: {elapsed}.");


    // Format and display the TimeSpan value.
    Console.WriteLine(
        value: "RunTime: horas, minutos, segundos, milesimos de segundos");
    var elapsedTime =
        $"{elapsed.Hours:00}:{elapsed.Minutes:00}:" +
        $"{elapsed.Seconds:00}.{elapsed.Milliseconds:00}";

    Console.WriteLine(value: "RunTime: " + elapsedTime);

    TimeTracker.PrintTimerToConsole(timerName: TimeTracker.SeederTimerName);

    // Thread.Sleep(3000);
}


// Método para verificar que processos ainda estão em execução
static void RunningProcessInTheApp()
{
    // Obter o ID do processo atual
    var processId = Process.GetCurrentProcess().Id;

    // Obter o ID do processo atual
    var processName = Process.GetCurrentProcess().ProcessName;

    // Obter o objeto Process para o processo atual
    var process = Process.GetProcessById(processId: processId);

    // Obter a coleção de threads associados ao processo
    var threadCollection = process.Threads;

    // Exibir informações sobre cada thread
    var message =
        $"Threads em execução no processo " +
        $"{process.ProcessName} (ID: {process.Id}):";

    Console.WriteLine(value: message);

    ProcessList.ListOfProcesses.Add(item: message);

    foreach (ProcessThread thread in threadCollection)
    {
        message = $"Thread ID: {thread.Id}, Estado: {thread.ThreadState}";

        Console.WriteLine(value: message);

        ProcessList.ListOfProcesses.Add(item: message);
    }
}


// Método para verificar que processos ainda estão em execução
static void StartProgramTimer()
{
    // calcular o tempo decorrido para executar o método principal

    // Create a new timer with the name from variable "timerName" 
    TimeTracker.CreateTimer(timerName: TimeTracker.AppBuilderTimerName);

    // Start the timer "MyTimer"
    TimeTracker.StartTimer(timerName: TimeTracker.AppBuilderTimerName);
}


// Método para verificar que processos ainda estão em execução
static void StopProgramTimer()
{
    // Stop the timer "MyTimer"
    TimeTracker.StopTimer(timerName: TimeTracker.AppBuilderTimerName);


    // Get the elapsed time for the timer "MyTimer"
    var elapsed =
        TimeTracker.GetElapsedTime(timerName: TimeTracker.AppBuilderTimerName);


    // Format and display the TimeSpan value.
    // Console.WriteLine(
    //     "RunTime: horas, minutos, segundos, milesimos de segundos");
    //
    // var elapsedTime =
    //     $"{elapsed.Hours:00}:{elapsed.Minutes:00}:{elapsed.Seconds:00}." +
    //     $"{elapsed.Milliseconds:00}";
    //
    // Console.WriteLine("RunTime (Program builder time): " + elapsedTime);

    TimeTracker.PrintTimerToConsole(timerName: TimeTracker.AppBuilderTimerName);
}


// -------------------------------------------------------------------------- //


StartProgramTimer();

// -------------------------------------------------------------------------- //

// Configuring Azure services for Blob and Queue clients.
builder.Services.AddAzureClients(configureClients: clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(
        serviceUriOrConnectionString: builder.Configuration[key: "AzureStorage:SchoolProjectStorageNuno:blob"],
        preferMsi: true);
    clientBuilder.AddQueueServiceClient(
        serviceUriOrConnectionString: builder.Configuration[key: "AzureStorage:SchoolProjectStorageNuno:queue"],
        preferMsi: true);
});


// -------------------------------------------------------------------------- //


// -------------------------------------------------------------------------- //
// Configuring database connections using DbContext for MSSQL, MySQL, and SQLite.
// -------------------------------------------------------------------------- //
builder.Services.AddDbContext<DataContextMsSql>(
    optionsAction: cfg =>
    {
        cfg.UseSqlServer(
            connectionString: builder.Configuration.GetConnectionString(
                name: builder.Environment.IsDevelopment()
                    // Usar a Connection String local durante o desenvolvimento
                    ? "SP-MSSql-Somee"
                    // Usar a Connection String online ao publicar
                    : "SP-SmarterASP-MSSQL") ?? string.Empty,
            sqlServerOptionsAction: options =>
            {
                options.EnableRetryOnFailure();
                options.MigrationsAssembly(assemblyName: "SchoolProject.Web");
                options.MigrationsHistoryTable(tableName: "_MyMigrationsHistory");
            }).EnableSensitiveDataLogging();
    });


// -------------------------------------------------------------------------- //

builder.Services.AddDbContext<DataContextMySql>(
    optionsAction: cfg =>
    {
        cfg.UseMySQL(
            connectionString: builder.Configuration.GetConnectionString(
                name: builder.Environment.IsDevelopment()
                    // Usar a Connection String local durante o desenvolvimento
                    ? "SP-MySQL-Local"
                    // Usar a Connection String online ao publicar
                    : "SP-SmarterASP-MySQL") ?? string.Empty,
            mySqlOptionsAction: options =>
            {
                options.MigrationsAssembly(assemblyName: "SchoolProject.Web");
                options.MigrationsHistoryTable(tableName: "_MyMigrationsHistory");
            }).EnableSensitiveDataLogging();
    });

// -------------------------------------------------------------------------- //

builder.Services.AddDbContext<DataContextSqLite>(
    optionsAction: cfg =>
    {
        cfg.UseSqlite(
            connectionString: builder.Configuration.GetConnectionString(
                name: builder.Environment.IsDevelopment()
                    // Usar a Connection String local durante o desenvolvimento
                    ? "SP-SQLite-Local"
                    // Usar a Connection String online ao publicar
                    : "SP-SQLite-Online") ?? string.Empty,
            sqliteOptionsAction: options =>
            {
                options.MigrationsAssembly(assemblyName: "SchoolProject.Web");
                options.MigrationsHistoryTable(tableName: "_MyMigrationsHistory");
            }).EnableSensitiveDataLogging();
    });


// -------------------------------------------------------------------------- //

// Configure Identity service with user settings,
// password settings, and token settings.
// builder.Services.AddIdentity<IdentityUser, IdentityRole>(
builder.Services.AddIdentity<User, IdentityRole>(
        setupAction: cfg =>
        {
            // User settings.
            cfg.User.RequireUniqueEmail = true;

            // Password settings.
            cfg.Password.RequireDigit = true;
            cfg.Password.RequiredLength = 8;
            cfg.Password.RequiredUniqueChars = 0;
            cfg.Password.RequireUppercase = true;
            cfg.Password.RequireLowercase = true;
            cfg.Password.RequireNonAlphanumeric = false;

            // SignIn settings.
            cfg.SignIn.RequireConfirmedEmail = true;
            cfg.SignIn.RequireConfirmedAccount = false;
            cfg.SignIn.RequireConfirmedPhoneNumber = false;

            // Token settings.
            cfg.Tokens.AuthenticatorTokenProvider =
                TokenOptions.DefaultAuthenticatorProvider;
        })
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<DataContextMySql>()
    .AddEntityFrameworkStores<DataContextMsSql>()
    .AddEntityFrameworkStores<DataContextSqLite>();


// Configure Application Insights for telemetry.
builder.Services.AddApplicationInsightsTelemetry(options: options =>
{
    options.ConnectionString = "APPLICATIONINSIGHTS_CONNECTION_STRING";
});


// Configure JWT authentication.
builder.Services.AddAuthentication(defaultScheme: JwtBearerDefaults.AuthenticationScheme)
    .AddCookie()
    .AddJwtBearer(configureOptions: options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration[key: "Tokens:Issuer"],
            ValidAudience = builder.Configuration[key: "Tokens:Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    key: Encoding.UTF8.GetBytes(
                        s: builder.Configuration[key: "Tokens:Key"] ??
                           GenerateRandomString(length: 128, withSpecialCharacters: false)
                    )
                )
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() ==
                    typeof(SecurityTokenExpiredException))
                    context.Response.Headers.Add(key: "Token-Expired", value: "true");

                return Task.CompletedTask;
            }
        };
    });


// Configure Cookie authentication.
builder.Services.AddAuthentication(defaultScheme: "CookieAuth")
    .AddCookie(authenticationScheme: "CookieAuth",
        configureOptions: config =>
        {
            config.Cookie.Name = "SchoolProject.Web.Cookie";
            config.LoginPath = "/Home/Authenticate";
            config.AccessDeniedPath = "/Home/AccessDenied";
        });


// Configure application cookie settings with sliding expiration.
builder.Services.ConfigureApplicationCookie(configure: options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(value: 15);

    // options.LoginPath = "/Account/Login";
    // options.AccessDeniedPath = "/Account/NotAuthorized";

    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";

    options.SlidingExpiration = true;
});


// Configure consent cookie options.
builder.Services.Configure<CookiePolicyOptions>(configureOptions: options =>
{
    options.CheckConsentNeeded = _ => true;

    options.MinimumSameSitePolicy = SameSiteMode.None;

    options.ConsentCookie.IsEssential = true;
    options.ConsentCookie.Expiration = TimeSpan.FromDays(value: 30);
    options.ConsentCookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ConsentCookie.HttpOnly = true;
});


// de forma individual
//
// builder.Services.AddAuthentication().AddFacebook(facebookOptions =>
// {
//     facebookOptions.AppId =
//         builder.Configuration["Authentication:Facebook:AppId"];
//     facebookOptions.AppSecret =
//         builder.Configuration["Authentication:Facebook:AppSecret"];
// });


// de forma coletiva
//
builder.Services.AddAuthentication()
    .AddGoogle(configureOptions: options =>
    {
        var googleAuthSection =
            builder.Configuration.GetSection(key: "Authentication:Google");

        options.ClientId = googleAuthSection[key: "ClientId"];
        options.ClientSecret = googleAuthSection[key: "ClientSecret"];
    })
    .AddFacebook(configureOptions: options =>
    {
        var fbAuthSection =
            builder.Configuration.GetSection(key: "Authentication:Facebook");

        options.ClientId = fbAuthSection[key: "AppId"];
        options.ClientSecret = fbAuthSection[key: "AppSecret"];
    })
    .AddMicrosoftAccount(configureOptions: microsoftOptions =>
    {
        var googleAuthSection =
            builder.Configuration.GetSection(key: "Authentication:Microsoft");

        microsoftOptions.ClientId =
            builder.Configuration[key: "Authentication:Microsoft:ClientId"];

        microsoftOptions.ClientSecret =
            builder.Configuration[key: "Authentication:Microsoft:ClientSecret"];
    })
    .AddTwitter(configureOptions: twitterOptions =>
    {
        var googleAuthSection =
            builder.Configuration.GetSection(key: "Authentication:Twitter");

        twitterOptions.ConsumerKey =
            builder.Configuration[key: "Authentication:Twitter:ConsumerKey"];

        twitterOptions.ConsumerSecret =
            builder.Configuration[key: "Authentication:Twitter:ConsumerSecret"];

        twitterOptions.RetrieveUserDetails = true;
    });


// Add authorization policies for different user roles.
builder.Services.AddAuthorization(configure: options =>
{
    options.AddPolicy(name: "IsAdmin",
        configurePolicy: policy => policy.RequireClaim(claimType: "IsAdmin"));

    options.AddPolicy(name: "IsStudent",
        configurePolicy: policy => policy.RequireClaim(claimType: "IsStudent"));

    options.AddPolicy(name: "IsTeacher",
        configurePolicy: policy => policy.RequireClaim(claimType: "IsTeacher"));

    options.AddPolicy(name: "IsParent",
        configurePolicy: policy => policy.RequireClaim(claimType: "IsParent"));

    options.AddPolicy(name: "IsUser",
        configurePolicy: policy => policy.RequireClaim(claimType: "IsUser"));

    options.AddPolicy(name: "IsAnonymous",
        configurePolicy: policy => policy.RequireClaim(claimType: "IsAnonymous"));
});


// Add localization and view localization to the application.
builder.Services.AddLocalization(setupAction: options =>
    options.ResourcesPath = "Resources");


builder.Services.Configure<RequestLocalizationOptions>(configureOptions: options =>
{
    var supportedCultures = new[]
    {
        new CultureInfo(name: "en-US"),
        new CultureInfo(name: "pt-PT"),
        new CultureInfo(name: "pt-BR")
    };

    options.DefaultRequestCulture = new RequestCulture(culture: "en-US");

    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
});


builder.Services.AddMvc().AddViewLocalization();
builder.Services.AddMvcCore().AddViewLocalization();
builder.Services.AddRazorPages().AddMicrosoftIdentityUI().AddViewLocalization();


// Add logging providers for debugging and application insights.

// --------------------------------- --------------------------------------- //
// Configuration type 1
// --------------------------------- --------------------------------------- //
// Log.Logger = new LoggerConfiguration()
//
//     // Set default minimum log level
//     .MinimumLevel.Information()
//
//     // Set default minimum log level
//     .MinimumLevel.Debug()
//
//     // Add console (Sink) as logging target
//     .WriteTo.Console()
//
//     // Write logs to a file for warning and logs with a higher severity
//     // Logs are written in JSON
//     .WriteTo.File(new JsonFormatter(),
//         ".\\data\\important-logs.json",
//         restrictedToMinimumLevel: LogEventLevel.Warning)
//
//     // Add file (Sink) as logging target
//     // Add a log file that will be replaced by a new log file each day
//     .WriteTo.File(".\\data\\all-daily-.logs",
//         rollingInterval: RollingInterval.Day)
//
//     // Create the actual logger
//     .CreateLogger();
//
// builder.Services.AddLogging(cfg => { cfg.AddSerilog(); });
// --------------------------------- --------------------------------------- //


// --------------------------------- --------------------------------------- //
// Configuration type 2
// --------------------------------- --------------------------------------- //
var serilogConfig =
    builder.Configuration.GetSection(key: "Serilog");

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(configuration: serilogConfig)
    .CreateLogger();

builder.Services.AddLogging(configure: cfg =>
{
    cfg.AddSerilog(dispose: true); // Integrates Serilog as the logging provider
});

// --------------------------------- --------------------------------------- //

// builder.Services.AddLogging();

builder.Logging.AddDebug();
builder.Logging.AddConsole();
builder.Logging.AddEventSourceLogger();
builder.Logging.AddApplicationInsights();


// Configuração do serviço DatabaseConnectionVerifier
builder.Services.AddTransient<DatabaseConnectionVerifier>();


// Inject repositories and helpers.
// builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IEmailSender, EmailHelper>();
builder.Services.AddScoped<IImageHelper, ImageHelper>();
builder.Services.AddScoped<IStorageHelper, StorageHelper>();
builder.Services.AddScoped<IConverterHelper, ConverterHelper>();


// Add seeding for the database.
// builder.Services.AddTransient<SeedDb>();
builder.Services.AddScoped<SeedDb>();

// builder.Services.AddScoped<SeedDbUsers>();
// builder.Services.AddScoped<SeedDbStudentsAndTeachers>();
// builder.Services.AddScoped<SeedDbSchoolClasses>();

// builder.Services.AddScoped<SeedDbTeachersWithCourses>();
// builder.Services.AddScoped<SeedDbSchoolClassesWithCourses>();
// builder.Services.AddScoped<SeedDbStudentsWithSchoolClasses>();


// builder.Services.AddScoped<SeedDbSchoolClassStudents>();
// builder.Services.AddTransient<SeedDb>().BuildServiceProvider().GetService<SeedDb>();
// builder.Services.AddTransient<SeedDb>().Configure();
// builder.Services.AddTransient<SeedDbMsSql>();
// builder.Services.AddTransient<SeedDbMySql>();
// builder.Services.AddTransient<SeedDbSqLite>();


// Extrai o nome do servidor da Connection String
var serverHostName =
    GetServerHostNameFromConnectionString(
        connectionString: builder.Configuration.GetConnectionString(
            name: "SP-MSSql-Somee") ?? string.Empty);


// Verifica se o servidor está disponível
GetServerHostNamePing(serverHostName: serverHostName);


// -------------------------------------------------------------------------- //

// Build the application.
// Configure the HTTP request pipeline.
var app = builder.Build();


// ------------------- //
await RunSeeding(host: app);

// Exception handling and HTTPS redirection for non-development environments.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(errorHandlingPath: "/Home/Error");

    // The default HSTS value is 30 days.
    // You may want to change this for production scenarios,
    // see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


// 
// app.UseStatusCodePages();
app.UseStatusCodePagesWithRedirects("/error/{0}");
// app.UseStatusCodePagesWithReExecute("/error/{0}");


// Enable HTTPS redirection and serve static files.
app.UseHttpsRedirection();
app.UseStaticFiles();


// Configure routing, authentication, and authorization middle-ware.
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


// Map controller routes and Razor pages.
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// 
app.MapRazorPages();


// running processes
RunningProcessInTheApp();


// elapsed time for the program
StopProgramTimer();


// Run the application.
app.Run();