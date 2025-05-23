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
// Configuraci�n general
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
// Autenticaci�n JWT
// 
builder.Services.AddCustomJwtAuthentication(configuration);

// 
// Configuraci�n de Mapster
// 
builder.Services.AddMapster();
MapsterConfig.RegisterMappings(); // Registra los mapeos

// 
// Inyecci�n de dependencias (Servicios)
// 
builder.Services.AddApplicationServices();

// 
// Configuraci�n de la base de datos
//
builder.Services.AddCustomDataBase(configuration);


// <summary>
// Configuraci�n de CORS
// </summary>
builder.Services.AddCustomCors(builder.Configuration);

// 
//  Construcci�n de la app
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
app.UseCors("AllowFrontend");   // Aplica pol�tica de CORS
app.UseAuthentication();
app.UseAuthorization();         // Activa autorizaci�n

app.MapControllers();          // Mapea los controladores

app.Run();