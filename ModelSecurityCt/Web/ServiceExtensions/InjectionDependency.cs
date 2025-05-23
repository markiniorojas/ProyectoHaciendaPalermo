using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Data.Interfaces;
using Data.Repositories;
using Business.Services;
using Business.Token;
using Business.Interfaces;
using Email.Interface;
using Email.Mensajes;
using Shared.Interface;
using Web.ServicioLog;

namespace WebServiceExtensions
{
    public static class InjectionDependency
    {
            public static IServiceCollection AddApplicationServices(this IServiceCollection services)
            {

                services.AddScoped<IUnitOfWork, UnitOfWork>();
                services.AddScoped<IPersonRepository, PersonRepository>();
                services.AddScoped<PersonService>();

                services.AddScoped<IFormRepository, FormRepository>();
                services.AddScoped<FormService>();

                services.AddScoped<IModuleRepository, ModuleRepository>();
                services.AddScoped<ModuleService>();

                services.AddScoped<IPermissionRepository, PermissionRepository>();
                services.AddScoped<PermissionService>();

                services.AddScoped<IRolRepository, RolRepository>();
                services.AddScoped<RolService>();

                services.AddScoped<IUserRepository, UserRepository>();
                services.AddScoped<UserRepository>();
                services.AddScoped<IUserService, UserService>();

                services.AddScoped<IFormModuleRepository, FormModuleRepository>();
                services.AddScoped<FormModuleService>();

                services.AddScoped<IRolUserRepository, RolUserRepository>();
                services.AddScoped<RolUserService>();

                services.AddScoped<IRolFormPermissionRepository, RolFormPermissionRepository>();
                services.AddScoped<RolFormPermissionService>();

                services.AddScoped<RegistroRepository>();
                services.AddScoped<RegistroService>();

                services.AddScoped<generarToken>();
                
                //Inyectar la funcion de enviar mensajes
                services.AddScoped<IMensajeEmail, CorreoMensaje>();
                services.AddScoped<IMensajeTelegram, MensajeTelegram>();

                //Inyectar el servicio de log

                services.AddScoped<ICurrentRequestUserService, CurrentRequestUserService>();
            
            return services;
        }

    }
}
