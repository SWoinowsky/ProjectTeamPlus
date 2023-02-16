using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SteamProject.Models;

var builder = WebApplication.CreateBuilder(args);

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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
