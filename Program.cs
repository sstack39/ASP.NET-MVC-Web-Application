using Microsoft.EntityFrameworkCore;
using StackWebApp.Data;
using StackWebApp.Models;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using StackWebApp;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

// Add other services here
builder.Services.AddControllers();
builder.Services.AddRazorPages();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
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

app.MapControllers();
app.MapRazorPages();

// Ensure the database is migrated and seeded
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await MigrateDatabaseAndSeedUsers(services);
}

// Add this line to log the URLs the app is listening on
Console.WriteLine($"Application is running on: {string.Join(", ", app.Urls)}");

app.Run();

static async Task MigrateDatabaseAndSeedUsers(IServiceProvider services)
{
    var context = services.GetRequiredService<ApplicationDbContext>();
    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

    await context.Database.EnsureCreatedAsync();

    await context.Database.MigrateAsync();

    if (!await roleManager.RoleExistsAsync("Admin"))
    {
        await roleManager.CreateAsync(new IdentityRole("Admin"));
    }

    await CreateUserIfNotExists(userManager, "admin@example.com", "AdminPassword123!", "Admin", "User", true, "Admin");
    await CreateUserIfNotExists(userManager, "testuser@example.com", "TestPassword123!", "Test", "User", false);

    await context.SaveChangesAsync();
}

static async Task CreateUserIfNotExists(UserManager<ApplicationUser> userManager, string email, string password, string firstName, string lastName, bool emailConfirmed, string role = null!)
{
    if (await userManager.FindByEmailAsync(email) == null)
    {
        var user = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            EmailConfirmed = emailConfirmed
        };

        var result = await userManager.CreateAsync(user, password);

        if (result.Succeeded)
        {
            Console.WriteLine($"User {email} created successfully.");
            if (role != null)
            {
                var roleResult = await userManager.AddToRoleAsync(user, role);
                if (roleResult.Succeeded)
                {
                    Console.WriteLine($"User {email} added to role {role}.");
                }
                else
                {
                    Console.WriteLine($"Failed to add user {email} to role {role}. Errors: {string.Join(", ", roleResult.Errors.Select(e => e.Description))}");
                }
            }
        }
        else
        {
            Console.WriteLine($"Failed to create user {email}. Errors: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
    else
    {
        Console.WriteLine($"User {email} already exists.");
    }
}
app.Run();