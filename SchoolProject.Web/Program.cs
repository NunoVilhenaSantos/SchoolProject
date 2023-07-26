using System.Globalization;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using Microsoft.Identity.Web.UI;
using Microsoft.IdentityModel.Tokens;
using SchoolProject.Web;
using SchoolProject.Web.Data.DataContexts;
using SchoolProject.Web.Data.Entities.ExtraEntities;
using SchoolProject.Web.Data.Seeders;
using SchoolProject.Web.Helpers.ConverterModelClassOrClassModel;
using SchoolProject.Web.Helpers.Email;
using SchoolProject.Web.Helpers.Images;
using SchoolProject.Web.Helpers.Storages;
using SchoolProject.Web.Helpers.Users;


// Helper method to generate a random
// string with specified length and characters.
static string GenerateRandomString(
    int length = 64, bool withSpecialCharacters = true)
{
    var characterSet =
        "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
    if (withSpecialCharacters) characterSet += "!@#$%^&*()_-+=[]{}|;:,.<>?";

    var random = new Random();
    var sb = new StringBuilder(length);

    for (var i = 0; i < length; i++)
    {
        var index = random.Next(characterSet.Length);
        var randomCharacter = characterSet[index];
        sb.Append(randomCharacter);
    }

    var randomString = sb.ToString();
    Console.WriteLine(
        randomString); // Print the generated string to the console
    return randomString;
}


// Create a new web application using the WebApplicationBuilder.
var builder = WebApplication.CreateBuilder(args);


// Configuring Azure services for Blob and Queue clients.
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(
        builder.Configuration["AzureStorage:SchoolProjectStorageNuno:blob"],
        true);
    clientBuilder.AddQueueServiceClient(
        builder.Configuration["AzureStorage:SchoolProjectStorageNuno:queue"],
        true);
});


// Configuring database connections using DbContext for MSSQL, MySQL, and SQLite.
builder.Services.AddDbContext<DataContextMsSql>(
    cfg =>
    {
        cfg.UseSqlServer(
            builder.Configuration.GetConnectionString(
                "SchoolProject-somee"), options =>
            {
                options.EnableRetryOnFailure();
                options.MigrationsAssembly("SchoolProject.Web");
                options.MigrationsHistoryTable("_MyMigrationsHistory");
            });
    });

builder.Services.AddDbContext<DataContextMySql>(
    cfg =>
    {
        cfg.UseMySQL(
            builder.Configuration.GetConnectionString("SchoolProject-MySQL") ??
            string.Empty, options =>
            {
                options.MigrationsAssembly("SchoolProject.Web");
                options.MigrationsHistoryTable("_MyMigrationsHistory");
            });
    });

builder.Services.AddDbContext<DataContextSqLite>(
    cfg =>
    {
        cfg.UseSqlite(
            builder.Configuration.GetConnectionString("SchoolProject-SQLite"),
            options =>
            {
                options.MigrationsAssembly("SchoolProject.Web");
                options.MigrationsHistoryTable("_MyMigrationsHistory");
            });
    });

// Configure Identity service with user settings,
// password settings, and token settings.
// builder.Services.AddIdentity<IdentityUser, IdentityRole>(
builder.Services.AddIdentity<User, IdentityRole>(
        cfg =>
        {
            // User settings.
            cfg.User.RequireUniqueEmail = true;

            // Password settings.
            cfg.Password.RequireDigit = false;
            cfg.Password.RequiredLength = 6;
            cfg.Password.RequiredUniqueChars = 0;
            cfg.Password.RequireUppercase = false;
            cfg.Password.RequireLowercase = false;
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
    .AddEntityFrameworkStores<DataContextMsSql>()
    .AddEntityFrameworkStores<DataContextMySql>()
    .AddEntityFrameworkStores<DataContextSqLite>();


// Configure Application Insights for telemetry.
builder.Services.AddApplicationInsightsTelemetry(options =>
{
    options.ConnectionString = "APPLICATIONINSIGHTS_CONNECTION_STRING";
});


// Configure JWT authentication.
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddCookie()
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Tokens:Issuer"],
            ValidAudience = builder.Configuration["Tokens:Audience"],
            IssuerSigningKey =
                new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(
                        builder.Configuration["Tokens:Key"] ??
                        GenerateRandomString(128, false)
                    )
                )
        };

        options.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = context =>
            {
                if (context.Exception.GetType() ==
                    typeof(SecurityTokenExpiredException))
                    context.Response.Headers.Add("Token-Expired", "true");

                return Task.CompletedTask;
            }
        };
    });


// Configure Cookie authentication.
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth",
        config =>
        {
            config.Cookie.Name = "SuperShop.Cookie";
            config.LoginPath = "/Home/Authenticate";
            config.AccessDeniedPath = "/Home/Authenticate";
        });


// Configure application cookie settings with sliding expiration.
builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromDays(15);
    options.LoginPath = "/Account/NotAuthorized";
    options.AccessDeniedPath = "/Account/NotAuthorized";
    options.SlidingExpiration = true;
});


// Configure consent cookie options.
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = _ => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.ConsentCookie.IsEssential = true;
    options.ConsentCookie.Expiration = TimeSpan.FromDays(30);
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
    .AddGoogle(options =>
    {
        IConfigurationSection googleAuthSection =
            builder.Configuration.GetSection("Authentication:Google");

        options.ClientId = googleAuthSection["ClientId"];
        options.ClientSecret = googleAuthSection["ClientSecret"];
    })
    .AddFacebook(options =>
    {
        IConfigurationSection fbAuthSection =
            builder.Configuration.GetSection("Authentication:Facebook");

        options.ClientId = fbAuthSection["AppId"];
        options.ClientSecret = fbAuthSection["AppSecret"];
    })
    .AddMicrosoftAccount(microsoftOptions =>
    {
        IConfigurationSection googleAuthSection =
            builder.Configuration.GetSection("Authentication:Microsoft");

        microsoftOptions.ClientId =
            builder.Configuration["Authentication:Microsoft:ClientId"];

        microsoftOptions.ClientSecret =
            builder.Configuration["Authentication:Microsoft:ClientSecret"];
    })
    .AddTwitter(twitterOptions =>
    {
        IConfigurationSection googleAuthSection =
            builder.Configuration.GetSection("Authentication:Twitter");

        twitterOptions.ConsumerKey =
            builder.Configuration["Authentication:Twitter:ConsumerKey"];

        twitterOptions.ConsumerSecret =
            builder.Configuration["Authentication:Twitter:ConsumerSecret"];

        twitterOptions.RetrieveUserDetails = true;
    });


// Add authorization policies for different user roles.
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("IsAdmin",
        policy => policy.RequireClaim("IsAdmin"));

    options.AddPolicy("IsStudent",
        policy => policy.RequireClaim("IsStudent"));

    options.AddPolicy("IsTeacher",
        policy => policy.RequireClaim("IsTeacher"));

    options.AddPolicy("IsParent",
        policy => policy.RequireClaim("IsParent"));

    options.AddPolicy("IsUser",
        policy => policy.RequireClaim("IsUser"));

    options.AddPolicy("IsAnonymous",
        policy => policy.RequireClaim("IsAnonymous"));
});




// Add localization and view localization to the application.
builder.Services.AddLocalization(options =>
    options.ResourcesPath = "Resources");


builder.Services.Configure<RequestLocalizationOptions>(options =>
    {
        var supportedCultures = new[]
        {
            new CultureInfo("en-US"),
            new CultureInfo("pt-PT"),
            new CultureInfo("pt-BR"),
        };

        options.DefaultRequestCulture = new RequestCulture("en-US");

        options.SupportedCultures = supportedCultures;
        options.SupportedUICultures = supportedCultures;
    });






builder.Services.AddMvc().AddViewLocalization();
builder.Services.AddMvcCore().AddViewLocalization();
builder.Services.AddRazorPages().AddMicrosoftIdentityUI().AddViewLocalization();


// Add logging providers for debugging and application insights.
builder.Services.AddLogging();
builder.Logging.AddDebug();
builder.Logging.AddConsole();
builder.Logging.AddEventSourceLogger();
builder.Logging.AddApplicationInsights();


// Add seeding for the database.
builder.Services.AddTransient<SeedDb>();
// builder.Services.AddTransient<SeedDbMsSql>();
// builder.Services.AddTransient<SeedDbMySql>();
// builder.Services.AddTransient<SeedDbSqLite>();


// Inject repositories and helpers.
builder.Services.AddScoped<UserManager<User>>();
builder.Services.AddScoped<IUserHelper, UserHelper>();
builder.Services.AddScoped<IEmailSender, EmailHelper>();
builder.Services.AddScoped<IImageHelper, ImageHelper>();
builder.Services.AddScoped<IStorageHelper, StorageHelper>();
builder.Services.AddScoped<IConverterHelper, ConverterHelper>();

// Configure the HTTP request pipeline.
var app = builder.Build();


// Exception handling and HTTPS redirection for non-development environments.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");

    // The default HSTS value is 30 days.
    // You may want to change this for production scenarios,
    // see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


// Enable HTTPS redirection and serve static files.
app.UseHttpsRedirection();
app.UseStaticFiles();


// Configure routing, authentication, and authorization middleware.
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();


// Map controller routes and Razor pages.
app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();


// Run the application.
app.Run();