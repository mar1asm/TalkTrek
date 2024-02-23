using Learning_platform.DBContexts;
using Learning_platform.Entities;
using Microsoft.AspNetCore.Identity;
using Learning_platform.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Learning_platform.Configuration;
using System.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddDbContext<TutoringPlatformContext>(options => {
    options.UseMySQL(connectionString: builder.Configuration.GetConnectionString("DefaultConnection"));
    //options.LogTo(Console.WriteLine, LogLevel.Information);
});

builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@/";
})
.AddEntityFrameworkStores<TutoringPlatformContext>().AddDefaultTokenProviders();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Default Password settings.
    //options.User.UserName = options.User.Email;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;
});


// Check database connection during application startup
using (var scope = builder.Services.BuildServiceProvider().CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TutoringPlatformContext>();
    try
    {
        // Attempt to open a connection to the database
        dbContext.Database.OpenConnection();
        // Log a success message if the connection is opened successfully
        Console.WriteLine("Successfully connected to the database.");
    }
    catch (Exception ex)
    {
        // Log an error message if an exception occurs while opening the connection
        Console.WriteLine($"Error connecting to the database: {ex.Message}");
    }
    finally
    {
        // Close the connection
        dbContext.Database.CloseConnection();
    }
}

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<EmailSenderOptions>(builder.Configuration.GetSection("EmailSenderOptions"));

builder.Services.AddScoped<IEmailSender, EmailSender>();


builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITutorRepository, TutorRepository>();
builder.Services.AddScoped<IStudentRepository, StudentRepository>();

builder.Services.AddHttpClient<EmailConfirmationClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7048/");
    // Configure other HttpClient options if needed
});

builder.Logging.AddConsole();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    // Seed roles
    await SeedRoles(roleManager);
}

async Task SeedRoles(RoleManager<IdentityRole> roleManager)
{
    // Check if roles exist, if not, create them
    if (!await roleManager.RoleExistsAsync("Tutor"))
    {
        await roleManager.CreateAsync(new IdentityRole("Tutor"));
    }

    if (!await roleManager.RoleExistsAsync("Student"))
    {
        await roleManager.CreateAsync(new IdentityRole("Student"));
    }
}


app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();



app.Run();
