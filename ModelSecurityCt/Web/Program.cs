using Business;
using Business.Interfaces;
using Business.Services;
using Business.Token;
using Data.Interfaces;
using Data.Repositories;
using Entity.context;
using Entity.Model;
using Mapster;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Utilities;
using Utilities.Mapping;
using Web.ServiceExtensions;
using WebServiceExtensions;


var builder = WebApplication.CreateBuilder(args);

// 
// Configuración general
// 
var configuration = builder.Configuration;
var dbProvider = configuration["DatabaseProvider"];

// 
// Servicios base
// 
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddCustomSwagger();
builder.Services.AddHttpContextAccessor(); // Acceso al contexto HTTP

// 
// Autenticación JWT
// 
builder.Services.AddCustomJwtAuthentication(configuration);

// 
// Configuración de Mapster
// 
builder.Services.AddMapster();
MapsterConfig.RegisterMappings(); // Registra los mapeos

// 
// Inyección de dependencias (Servicios)
// 
builder.Services.AddApplicationServices();

// 
// Configuración de la base de datos
//
builder.Services.AddCustomDataBase(configuration);


// <summary>
// Configuración de CORS
// </summary>
builder.Services.AddCustomCors(builder.Configuration);

// 
//  Construcción de la app
// 
var app = builder.Build();

// 
// Middleware HTTP
// 
if (app.Environment.IsDevelopment())
{
    app.UseSwagger(); // Swagger solo en desarrollo
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();      // Redirige a HTTPS
app.UseCors("AllowFrontend");   // Aplica política de CORS
app.UseAuthentication();
app.UseAuthorization();         // Activa autorización

app.MapControllers();          // Mapea los controladores

app.Run();