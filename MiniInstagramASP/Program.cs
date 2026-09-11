using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using MiniInstagramASP.Data;
using MiniInstagramASP.Services;
using MiniInstagramEF.context;

var builder = WebApplication.CreateBuilder(args);

var authConnection = builder.Configuration.GetConnectionString("AuthDb") 
                     ?? throw new InvalidOperationException("Connection string 'AuthDb' not found.");
var appConnection = builder.Configuration.GetConnectionString("AppDb") 
                    ?? throw new InvalidOperationException("Connection string 'AppDb' not found.");

CreateDirForDbFile(authConnection);
CreateDirForDbFile(appConnection);

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlite(authConnection)); 

builder.Services.AddDbContext<MiniInstagramContext>(options =>
    options.UseSqlite(appConnection));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = 500 * 1024 * 1024;
});


builder.Services.AddRazorPages();
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<IEmailSender, DummyEmailSender>();

builder.Services.AddSingleton<FileManagerService>();

builder.Services.AddScoped<CurrentUserService>();
builder.Services.AddHttpContextAccessor(); 

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<MiniInstagramContext>();
    ctx.Database.Migrate();
}

using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<AuthDbContext>();
    ctx.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "root",
    pattern: "",
    defaults: new { controller = "Feed", action = "Load" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Feed}/{action=Load}/{id?}");

app.MapGet("/", () => Results.Redirect("/feed"));

app.MapRazorPages();

app.Run();

return;


static void CreateDirForDbFile(string connectionString)
{
    var builder = new SqliteConnectionStringBuilder(connectionString);
    
    var databasePath = builder.DataSource;
    
    var directory = Path.GetDirectoryName(databasePath);

    if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
    {
        Directory.CreateDirectory(directory);
    }
}