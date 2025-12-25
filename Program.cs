using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ProjetNet.Data;
using ProjetNet.Models;
using ProjetNet.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<AppDbContext>() // utilise AppDbContext pour stocker les utilisateurs
    .AddDefaultTokenProviders(); // nécessaire pour reset password, email confirmation, etc.

// Register Gemini AI Service
builder.Services.AddHttpClient<IGeminiAIService, GeminiAIService>();

builder.Services.AddControllersWithViews();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AppDbContext>();

    Console.WriteLine("Initializing Database...");
    context.Database.EnsureCreated(); // créer la base si elle n'existe pas
    Console.WriteLine("Database Created/Ensured.");
    SeedData.Initialize(context);     // remplir les tables de test
    Console.WriteLine("Seed Data Completed.");
}

if (!app.Environment.IsDevelopment())
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
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);


app.Run();
