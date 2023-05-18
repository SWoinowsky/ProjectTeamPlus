using System.Security.Claims;
using AspNet.Security.OpenId;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SteamProject.Services;
using SteamProject.Models;
using SteamProject.DAL.Abstract;
using SteamProject.DAL.Concrete;
using Microsoft.AspNetCore.Identity.UI.Services;
using SteamProject.Areas.Identity.Data;
using OpenAI.GPT3.Extensions;
using SteamProject.Data;
using SteamProject.Utilities;
using System.Reflection;
using SteamProject.Models.Awards.Abstract;

var builder = WebApplication.CreateBuilder(args);

const bool localDbSource = true;
const bool azurePublish = !localDbSource;
// Add services to the container.

//Local Connection Strings
//Local Connection Strings
if (localDbSource == true)
{
    var connectionString = builder.Configuration.GetConnectionString("AuthenticationConnection") ?? throw new InvalidOperationException("Connection string 'AuthenticationConnection' not found.");
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseLazyLoadingProxies().UseSqlServer(connectionString));

    var connectionStringTwo = builder.Configuration.GetConnectionString("SteamInfoConnection") ?? throw new InvalidOperationException("Connection string 'SteamInfoConnection' not found.");
    builder.Services.AddDbContext<SteamInfoDbContext>(options =>
        options.UseLazyLoadingProxies().UseSqlServer(connectionStringTwo));
}

//Azure Connection Strings
if (localDbSource == false)
{
    if (azurePublish == true)
    {
        var connectionString = builder.Configuration.GetConnectionString("SteamInfoAuthConnectionAzure") ?? throw new InvalidOperationException("Connection string 'AuthenticationConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseLazyLoadingProxies().UseSqlServer(connectionString));

        var connectionStringTwo = builder.Configuration.GetConnectionString("SteamInfoConnectionAzure") ?? throw new InvalidOperationException("Connection string 'SteamInfoConnection' not found.");
        builder.Services.AddDbContext<SteamInfoDbContext>(options =>
            options.UseLazyLoadingProxies().UseSqlServer(connectionStringTwo));
    }
    else
    {
        var stringBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("SteamInfoConnectionAzure"))
        {
            Password = builder.Configuration["SteamInfo:DBPassword"]
        };
        builder.Services.AddDbContext<SteamInfoDbContext>(options =>
            options.UseLazyLoadingProxies().UseSqlServer(stringBuilder.ConnectionString));

        var authStringBuilder = new SqlConnectionStringBuilder(builder.Configuration.GetConnectionString("SteamInfoAuthConnectionAzure"))
        {
            Password = builder.Configuration["SteamInfo:DBPassword"]
        };
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseLazyLoadingProxies().UseSqlServer(authStringBuilder.ConnectionString));
    }
}


var SteamApiToken = builder.Configuration["SteamKey"];
var openAiToken = builder.Configuration["OpenAiKey"];
var AdminSeedingApiToken = builder.Configuration["AdminSeedingApiKey"];
var ClientId = builder.Configuration["ClientId"];
var AccessToken = builder.Configuration["AccessToken"];

builder.Services.AddScoped<ISteamService, SteamService>( s => new SteamService( SteamApiToken, AdminSeedingApiToken, ClientId, AccessToken));
builder.Services.AddScoped<IOpenAiApiService, OpenAiApiService>(a => new OpenAiApiService(openAiToken));
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserGameInfoRepository, UserGameInfoRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<IFriendRepository, FriendRepository>();
builder.Services.AddScoped<IGameAchievementRepository, GameAchievementRepository>();
builder.Services.AddScoped<IUserAchievementRepository, UserAchievementRepository>();
builder.Services.AddScoped<ICompetitionRepository, CompetitionRepository>();
builder.Services.AddScoped<ICompetitionPlayerRepository, CompetitionPlayerRepository>();
builder.Services.AddScoped<ICompetitionGameAchievementRepository, CompetitionGameAchievementRepository>();
builder.Services.AddScoped<IBlackListRepository, BlackListRepository>();
builder.Services.AddScoped<IBadgeRepository, BadgeRepository>();
builder.Services.AddScoped<IUserBadgeRepository, UserBadgeRepository>();
builder.Services.AddScoped<IInboxRepository, InboxRepository>();
builder.Services.AddScoped<IIGDBGenresRepository, IGDBGenresRepository>();
builder.Services.AddScoped<IStatusRepository, StatusRepository>();
builder.Services.AddScoped<ICompetitionVoteRepository, CompetitionVoteRepository>();
builder.Services.AddScoped<IGameVoteRepository, GameVoteRepository>();


builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()                   // enable roles
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.LoginPath = "/Identity/Account/Login";
        options.LogoutPath = "/Identity/Account/Logout";
    })
    .AddSteam(options =>
                    {
                        options.CorrelationCookie.SameSite = SameSiteMode.None;
                        options.CorrelationCookie.SecurePolicy = CookieSecurePolicy.Always;
                    });

builder.Services.AddTransient<IEmailSender, EmailSender>();
builder.Services.AddSingleton<IEmailSender, EmailSender>();

builder.Services.AddTransient<IInboxService, InboxService>();

builder.Services.AddOpenAIService();

builder.Services.AddControllersWithViews();
builder.Services.AddSwaggerGen();

var assembly = Assembly.GetExecutingAssembly();
var awardConditionTypes = assembly.GetTypes()
    .Where(t => t.GetInterfaces().Contains(typeof(IAwardCondition)) && !t.IsAbstract);

foreach (var type in awardConditionTypes)
{
    builder.Services.AddScoped(typeof(IAwardCondition), type);
}

var app = builder.Build();

// By using a scope for the services to be requested below, we limit their lifetime to this set of calls.
// See: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection?view=aspnetcore-5.0#call-services-from-main
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        // Get the IConfiguration service that allows us to query user-secrets and 
        // the configuration on Azure
        var config = app.Services.GetRequiredService<IConfiguration>();
        // Set password with the Secret Manager tool, or store in Azure app configuration
        // dotnet user-secrets set SeedUserPW <pw>

        var dbContext = services.GetRequiredService<SteamInfoDbContext>();
        SeedGameData.Seed(dbContext);

        var testUserPw = config["SeedUserPW"];

        SeedUsers.Initialize(services, SeedData.UserSeedData, testUserPw).Wait();
        var adminPw = config["SeedAdminPW"];

        SeedUsers.InitializeAdmin(services, "admin@example.com", "admin", adminPw, "My", "Admin").Wait();

        //also seed badges from json file
        SeedBadges.Initialize(services).GetAwaiter().GetResult();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred seeding the DB.");
    }
}

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
app.UseMiddleware<SteamProject.Middlewares.ThemeMiddleware>();


app.MapControllerRoute(
    "Dashboard",
    "Dashboard/",
    defaults: new { controller = "Home", action = "Dashboard" }
);

app.MapControllerRoute(
    "Compete",
    "Compete",
    defaults: new { controller = "Compete", action = "Index" }
);

app.MapControllerRoute(
    "Compete",
    "Compete/Create",
    defaults: new { controller = "Compete", action = "Create" }
);

app.MapControllerRoute(
    "Compete",
    "Compete/CreateSpeedRun",
    defaults: new { controller = "Compete", action = "CreateSpeedRun" }
);

app.MapControllerRoute(
    "Compete",
    "Compete/Details/{compId?}",
    defaults: new { controller = "Compete", action = "Details" }
);

app.MapControllerRoute(
    "Compete",
    "Compete/{friendSteamId?}/{appId?}",
    defaults: new { controller = "Compete", action = "Initiate" }
);

app.MapControllerRoute(
    "Friend",
    "Friend/{friendSteamId?}",
    defaults: new { controller = "Friend", action = "Index" }
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


app.MapRazorPages();

app.Run();
