using Learning_platform.DBContexts;
using Learning_platform.Entities;
using Microsoft.AspNetCore.Identity;
using Learning_platform.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Learning_platform.Configuration;
using System.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

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
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;


    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
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
builder.Services.Configure<JwtConfig>(builder.Configuration.GetSection("Jwt"));

builder.Services.AddScoped<IEmailSender, EmailSender>();
builder.Services.AddScoped<Security>();


builder.Services.AddScoped< UserRepository>();
builder.Services.AddScoped< TutorRepository>();
builder.Services.AddScoped< StudentRepository>();
builder.Services.AddScoped<MessageRepository>();
builder.Services.AddScoped<LanguageRepository>();

builder.Services.AddHttpClient<EmailConfirmationClient>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7048/");
    // Configure other HttpClient options if needed
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOrigin",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});

builder.Logging.AddConsole();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddAuthentication(options => {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["Jwt:JwtIssuer"],
                    ValidAudience = builder.Configuration["Jwt:JwtAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:JwtKey"]))
                };
            });

// Configure authorization policy
builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("RequireTutorRole", policy =>
                policy.RequireRole("Tutor"));

            options.AddPolicy("RequireStudentRole", policy =>
                policy.RequireRole("Student"));
        });


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

using (var scope = app.Services.CreateScope())
{
    var messageRepository = scope.ServiceProvider.GetRequiredService<MessageRepository>();
    await SeedMessageTypes(messageRepository);
}

using (var scope = app.Services.CreateScope())
{
    var languageRepository = scope.ServiceProvider.GetRequiredService<LanguageRepository>();
    await SeedTeachingCategories(languageRepository);
}


async Task SeedMessageTypes(MessageRepository messageRepository)
{
    if (await messageRepository.GetContentTypeAsync("Text") == null)
    {
        await messageRepository.CreateContentTypeAsync("Text");
    }

    if (await messageRepository.GetContentTypeAsync("Image") == null)
    {
        await messageRepository.CreateContentTypeAsync("Image");
    }

    if (await messageRepository.GetContentTypeAsync("Video") == null)
    {
        await messageRepository.CreateContentTypeAsync("Video");
    }

    if (await messageRepository.GetContentTypeAsync("Voice message") == null)
    {
        await messageRepository.CreateContentTypeAsync("Voice message");
    }


    // Add other content types similarly if needed
}

async Task SeedTeachingCategories (LanguageRepository languageRepository)
{

    if (await languageRepository.GetTeachingCategoryAsync("Conversational") == null)
    {
        await languageRepository.CreateTeachingCategoryAsync("Conversational");
    }
    if (await languageRepository.GetTeachingCategoryAsync("Business") == null)
    {
        await languageRepository.CreateTeachingCategoryAsync("Business");
    }
    if (await languageRepository.GetTeachingCategoryAsync("Language exam") == null)
    {
        await languageRepository.CreateTeachingCategoryAsync("Language exam");
    }
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

app.UseCors("AllowOrigin");


app.UseRouting();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();



app.Run();
