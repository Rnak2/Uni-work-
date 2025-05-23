using System.Reflection; //to access XML comments from the assembly
using Microsoft.OpenApi.Models; //to configure OpenAPI (Swagger) metadata
using System.Security.Claims; //to enable role-based claim policies
using RobotControllerApi.Services; //to register custom services
using Microsoft.AspNetCore.Authentication; //to configure custom authentication
using RobotControllerApi.Persistence;
using Microsoft.AspNetCore.Identity;


var builder = WebApplication.CreateBuilder(args);

//dependency injection setup
builder.Services.AddControllers();

//register the custom IUserService implementation
builder.Services.AddScoped<UserDataAccess>();
builder.Services.AddScoped<IUserService, UserService>();

//register Basic authentication using the custom handler
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>(
        "BasicAuthentication", options => { });

//authorization with custom policies based on roles
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin"));

    options.AddPolicy("UserOnly", policy =>
        policy.RequireClaim(ClaimTypes.Role, "Admin", "User"));
});

//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Robot Controller API",
        Description = "REST API for controlling and documenting moon robots.",
        Contact = new OpenApiContact
        {
            Name = "Rathanak Sambo",
            Email = "s223161551@deakin.edu.au"
        }
    });

    //enable XML comments
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));

    //add Basic Authentication definition
    options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        Description = "Enter your email and password"
    });

    //apply Basic authentication globally
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "basic",
                    Type = ReferenceType.SecurityScheme
                }
            },
            Array.Empty<string>()
        }
    });
});


var app = builder.Build();

app.UseSwagger(); //serve swagger.json
app.UseStaticFiles(); //css images
app.UseSwaggerUI(setup =>
{
    setup.InjectStylesheet("/styles/theme-flattop.css");
});

app.UseHttpsRedirection();
app.UseAuthentication(); //aply the basic authentication handler
app.UseAuthorization(); //rnforce policy based access control

//root message
app.MapGet("/", () => "Welcome to the Robot Controller API!");

//map controllers to route
app.MapControllers();

//seed admin account on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var config = services.GetRequiredService<IConfiguration>();
    var dataAccess = new RobotControllerApi.Persistence.UserDataAccess(config); // make sure you use correct namespace

    var adminEmail = "admin@deakin.edu.au";

    //check if admin exists
    var existingAdmin = await dataAccess.GetUserByEmailAsync(adminEmail);
    if (existingAdmin == null)
    {
        var hasher = new PasswordHasher<UserModel>();
        var adminUser = new UserModel
        {
            Email = adminEmail,
            FirstName = "System",
            LastName = "Admin",
            Description = "first admin account",
            Role = "Admin",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        //hash the password
        adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin123");

        //insert into db
        await dataAccess.AddUserAsync(adminUser);

        Console.WriteLine("Default admin account created.");
    }
    else
    {
        Console.WriteLine("Admin account already exists.");
    }
}


app.Run();
