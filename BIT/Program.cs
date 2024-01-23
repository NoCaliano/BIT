using BIT.Areas.Identity.Data;
using BIT.Data;
using BIT.DataStuff;
using BIT.FirstSetup;
using BIT.Hubs;
using BIT.Interfaces;
using BIT.Mocks;
using BIT.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("AuthDbContextConnection") ?? throw new InvalidOperationException("Connection string 'AuthDbContextConnection' not found.");

builder.Services.AddDbContext<AuthDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(connectionString));
builder.Services.AddScoped<IAllDishes, MockDish>();
builder.Services.AddScoped<IAllUsers, MockUser>();
builder.Services.AddScoped<IAllRoles, MockRole>();
builder.Services.AddScoped<IAllOrders, MockOrder>();
builder.Services.AddScoped<IAllCouriers, MockCourier>();
builder.Services.AddScoped<IAllCategories, MockCategory>();
builder.Services.AddScoped<IAllRequisitions, MockRequisition>();
builder.Services.AddScoped<IAllDishData, MockData>();
builder.Services.AddScoped<OrderProcessingService>();
builder.Services.AddHostedService<OrderProcessingHostedService>();
builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()  // Add roles configuration
    .AddEntityFrameworkStores<AuthDbContext>()
    .AddDefaultTokenProviders();

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
    string adminMail = "admin@gmail.com";
    string password = "Admin123!";

    
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var roles = new[] { "Admin", "Courier", "User" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    if (await userManager.FindByEmailAsync(adminMail) == null)
    {
        var user = new ApplicationUser();
        user.Email = adminMail;
        user.UserName = adminMail;

        await userManager.CreateAsync(user, password);
        await userManager.AddToRoleAsync(user, "Admin");

    }

    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var firstData = new FirstData();

    foreach (var dish in firstData.fdish)
    {
        // Check if the dish with the same name doesn't exist in the database
        if (!dbContext.Dishes.Any(d => d.Name == dish.Name))
        {
            dbContext.Dishes.Add(dish);
        }
    }

    foreach (var category in firstData.fcat)
    {
        // Check if the category with the same name doesn't exist in the database
        if (!dbContext.Categories.Any(c => c.Name == category.Name))
        {
            dbContext.Categories.Add(category);
        }
    }

    dbContext.SaveChanges();
}


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Welcome}/{action=Home}/{id?}");


// It just work
app.UseEndpoints(endpoints =>
{
    endpoints.MapHub<ChatHub>("/cartHub");
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
});


app.Run();
