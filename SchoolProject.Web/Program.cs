using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Azure;
using SchoolProject.Web;
using SchoolProject.Web.Data.DataContexts;


var builder = WebApplication.CreateBuilder(args);


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


// Add services to the container.
builder.Services.AddControllersWithViews();
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

builder.Services.AddDbContext<DataContextMSSQL>(options =>
    options.UseSqlServer(
        builder.Configuration
            .GetConnectionString("SchoolProject-mssql.somee.com")));


builder.Services.AddDbContext<DataContextMySQL>(options =>
    options.UseMySQL(
        builder.Configuration
            .GetConnectionString("SchoolProject-MySQL")));



builder.Services.AddDbContext<DataContextSQLite>(options =>
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