using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Akos.Data;
using Akos.DataStuff;
using System.Configuration;
using Akos.Interfaces;
using Akos.Mocks;
using Akos.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AuthDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AuthDbContextConnection' not found.");
var connectionString0 = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");


builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString0));
builder.Services.AddScoped<IAllDishes, MockDish>();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();



builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Welcome}/{action=Home}/{id?}");
app.MapRazorPages();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Courier", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

    string aemail = "AdminVanya@gmail.com";
    string apass = "VanyaC00lGuy!";

    if (await userManager.FindByEmailAsync(aemail) == null)
    {
        var user = new IdentityUser();
        user.UserName = aemail;
        user.Email = aemail;
        await userManager.CreateAsync(user, apass);
        await userManager.AddToRoleAsync(user, "Admin");
    }
}



app.Run();
