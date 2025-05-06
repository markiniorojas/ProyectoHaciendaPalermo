using Business;
using Business.Interfaces;
using Business.Services;
using Data.Interfaces;
using Data.Repositories;
using Entity.context;
using Entity.Model;
using Mapster;
using Microsoft.EntityFrameworkCore;
using Utilities.Mapping;
using Utilities;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Obtener configuración desde appsettings.json
var configuration = builder.Configuration;
var dbProvider = configuration["DatabaseProvider"];


// Servicios del negocio (Business Layer)

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddAuthorization();
builder.Services.AddAuthentication(config =>
{
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

}).AddJwtBearer(config =>
{
    config.RequireHttpsMetadata = false;
    config.SaveToken = true;
    config.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        IssuerSigningKey = new SymmetricSecurityKey
        (Encoding.UTF8.GetBytes(configuration["Jwt:Key"]!))
    };
});


builder.Services.AddScoped<IPersonRepository, PersonRepository>();
builder.Services.AddScoped<PersonService>();

builder.Services.AddScoped<IFormRepository, FormRepository>();
builder.Services.AddScoped<FormService>();

builder.Services.AddScoped<IModuleRepository, ModuleRepository>();
builder.Services.AddScoped<ModuleService>();

builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<PermissionService>();

builder.Services.AddScoped<IRolRepository, RolRepository>();
builder.Services.AddScoped<RolService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService,UserService>();

builder.Services.AddScoped<IFormModuleRepository, FormModuleRepository>();
builder.Services.AddScoped<FormModuleService>();
 
builder.Services.AddScoped<IRolUserRepository, RolUserRepository>();
builder.Services.AddScoped<RolUserService>();

builder.Services.AddScoped<IRolFormPermissionRepository, RolFormPermissionRepository>();
builder.Services.AddScoped<RolFormPermissionService>();

builder.Services.AddScoped<RegistroRepository>();
builder.Services.AddScoped<RegistroService>();

builder.Services.AddScoped<Jwt>();

// Registro de Mapster para mapeo de modelos
builder.Services.AddMapster();
MapsterConfig.RegisterMappings(); // Registra los mapeos

// Configuración de la base de datos
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    switch (dbProvider)
    {
        case "SqlServer":
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));
            break;
        case "MySql":
            options.UseMySql(configuration.GetConnectionString("MySqlConnection"),
                ServerVersion.AutoDetect(configuration.GetConnectionString("MySqlConnection")));
            break;
        case "Postgres":
            options.UseNpgsql(configuration.GetConnectionString("PostgresConnection"));
            break;
        default:
            throw new Exception("Proveedor de base de datos no soportado");
    }
});


// Configuración de CORS (para permitir acceso desde cualquier origen)

var origenesPermitidos = builder.Configuration
    .GetValue<string>("OrigenesPermitidos")!
    .Split(",");

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
    {
        policy.WithOrigins(origenesPermitidos) // Usar los orígenes configurados
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configuración de la canalización de solicitudes HTTP
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Habilitar Swagger solo en desarrollo
    app.UseSwaggerUI();
}

app.UseHttpsRedirection(); // Redirigir a HTTPS
app.UseCors("AllowFrontend"); // Habilitar CORS
app.UseAuthorization(); // Activar autorización

app.MapControllers(); // Mapear controladores

app.Run();