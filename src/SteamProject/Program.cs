using System.Security.Claims;
using AspNet.Security.OpenId;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using SteamProject.Models;
using SteamProject.Services;

var builder = WebApplication.CreateBuilder(args);
var token = builder.Configuration["steamkey"];

const bool localDbSource = true;
const bool azurePublish = false;
// Add services to the container.

//Local Connection Strings
if (localDbSource == true)
{
    var connectionString = builder.Configuration.GetConnectionString("AuthenticationConnection") ?? throw new InvalidOperationException("Connection string 'AuthenticationConnection' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(connectionString));

    var connectionStringTwo = builder.Configuration.GetConnectionString("SteamInfoConnection") ?? throw new InvalidOperationException("Connection string 'SteamInfoConnection' not found.");
    builder.Services.AddDbContext<SteamInfoDbContext>(options =>
        options.UseSqlServer(connectionStringTwo));

}

//Azure Connection Strings
if (localDbSource == false)
{
    if (azurePublish == true)
    {
        var connectionString = builder.Configuration.GetConnectionString("SteamInfoAuthConnectionAzure") ?? throw new InvalidOperationException("Connection string 'AuthenticationConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));

        var connectionStringTwo = builder.Configuration.GetConnectionString("SteamInfoConnectionAzure") ?? throw new InvalidOperationException("Connection string 'SteamInfoConnection' not found.");
        builder.Services.AddDbContext<SteamInfoDbContext>(options =>
            options.UseSqlServer(connectionStringTwo));
    }
    else
    {
        var stringBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("SteamInfoConnectionAzure"))
        {
            Password = builder.Configuration["SteamInfo:DBPassword"]
        };
        builder.Services.AddDbContext<SteamInfoDbContext>(options =>
            options.UseSqlServer(stringBuilder.ConnectionString));


        var authStringBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("SteamInfoAuthConnectionAzure"))
        {
            Password = builder.Configuration["SteamInfo:DBPassword"]
        };
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(authStringBuilder.ConnectionString));
    }

}


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.LoginPath = "/Identity/Account/Login";
        options.LogoutPath = "/Identity/Account/Logout";
    })
    .AddSteam();

builder.Services.AddScoped<DbContext, SteamInfoDbContext>();             // Need this line since our generic repository is based on DbContext directly
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));    // Easy way to register all the generic repositories 
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<ISteamServices, SteamServices>(s => new SteamServices(token));

builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
    
app.MapRazorPages();

app.Run();
