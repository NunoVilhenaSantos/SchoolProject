using Microsoft.ApplicationInsights.AspNetCore.Extensions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using SchoolProject.Web;
using SchoolProject.Web.Data.DataContexts;

var builder = WebApplication.CreateBuilder(args);


//
// Add Application Insights services into service collection
//
// builder.Services.AddApplicationInsightsTelemetry(
//     builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

// or
builder.Services.AddApplicationInsightsTelemetry(
    options => options.ConnectionString =
        "APPLICATIONINSIGHTS_CONNECTION_STRING"
);


// Configure JSON logging to the console.
// builder.Logging.AddJsonConsole();

// Configure logging to the console.
builder.Logging.AddDebug();
builder.Logging.AddConsole();
// builder.Logging.AddEventLog();
builder.Logging.AddEventSourceLogger();
builder.Logging.AddApplicationInsights();


builder.Services.AddControllersWithViews();


//builder.WebHost.ConfigureKestrel(options =>
//{
//    options.ListenAnyIP(9999, listenOptions =>
//    {
//        listenOptions.Protocols = HttpProtocols.Http1AndHttp2AndHttp3;

//        listenOptions.UseHttps(new TlsHandshakeCallbackOptions
//        {
//            OnConnection = context =>
//            {
//                var options = new SslServerAuthenticationOptions
//                {
//                    ServerCertificate =
//                         MyResolveCertForHost(context.ClientHelloInfo.ServerName)
//                };
//                return new ValueTask<SslServerAuthenticationOptions>(options);
//            },
//        });
//    });
//});


builder.Services.AddRazorPages();
builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
    options.ConsentCookie.IsEssential = true;
    options.ConsentCookie.Expiration = TimeSpan.FromDays(30);
    options.ConsentCookie.SecurePolicy = CookieSecurePolicy.Always;
    options.ConsentCookie.HttpOnly = true;
});


// -----------------------------------------------------------------------------
//
// Database connection via data-context
//
// -----------------------------------------------------------------------------


// Add services to the container.
builder.Services.AddAzureClients(clientBuilder =>
{
    clientBuilder.AddBlobServiceClient(
        builder.Configuration["AzureStorage:SchoolProjectStorageNuno:blob"],
        true);
    clientBuilder.AddQueueServiceClient(
        builder.Configuration["AzureStorage:SchoolProjectStorageNuno:queue"],
        true);
});


// builder.Services.AddDbContext<SchoolProjectDbContext>(options =>
//     options.UseSqlServer(
//         builder.Configuration.GetConnectionString("SchoolProjectConnection")));

builder.Services.AddDbContext<DataContextMssql>(options =>
    options.UseSqlServer(
        builder.Configuration
            .GetConnectionString("SchoolProject-mssql.somee.com")));


builder.Services.AddDbContext<DataContextMySql>(options =>
    options.UseMySQL(
        builder.Configuration
            .GetConnectionString("SchoolProject-MySQL") ?? string.Empty));


builder.Services.AddDbContext<DataContextSqLite>(options =>
    options.UseSqlite(
        builder.Configuration
            .GetConnectionString("SchoolProject-SQLite")));


builder.Services
    .AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Account/Login";
        options.LogoutPath = "/Account/Logout";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });


builder.Services.AddLogging();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "IsAdmin", policy => policy.RequireClaim("IsAdmin"));
    options.AddPolicy(
        "IsUser", policy => policy.RequireClaim("IsUser"));
});

builder.Services.AddAntiforgery();
builder.Services.AddApplicationInsightsTelemetry();


var app = builder.Build();

app.Logger.LogInformation("The app started");

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days.
    // You may want to change this for production scenarios,
    // see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    "default",
    "{controller=Home}/{action=Index}/{id?}");

app.Run();