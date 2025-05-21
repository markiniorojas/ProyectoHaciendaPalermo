using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection.Emit;

using System.Text;
using System.Threading.Tasks;
using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Dapper;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;
using Module = Entity.Model.Module;
using Entity.DataInit;
using Shared.Interface;
using System.Text.Json;


namespace Entity.context
{
    public class ApplicationDbContext : DbContext
    {
        protected readonly IConfiguration _configuration;

        private readonly ICurrentRequestUserService _currentRequestUserService;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration, ICurrentRequestUserService currentRequestUserService) : base(options)
        {
            _configuration = configuration;
            _currentRequestUserService = currentRequestUserService;
        }

        public DbSet<AudiLog> AuditLogs { get; set; }
        public DbSet<Person> Person { get; set; }
        public DbSet<Form> Form { get; set; }
        public DbSet<Rol> Role { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Module> Module { get; set; }
        public DbSet<User> user { get; set; }
        public DbSet<RolUser> RolUser { get; set; }
        public DbSet<FormModule> FormModule { get; set; }
        public DbSet<RolFormPermission> RolFormPermission { get; set; }



        // En la clase Entity.context.ApplicationDbContext
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.SeedPerson();
            modelBuilder.SeedUser();
            modelBuilder.SeedRol();
            modelBuilder.SeedRolUser();
            modelBuilder.SeedForm();
            modelBuilder.SeedPermission();
            modelBuilder.SeedModule();
            modelBuilder.SeedFormModule();
            modelBuilder.SeedRolFormPermission();
        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 2);
        }

        public override int SaveChanges()
        {
            EnsureAudit();
            return base.SaveChanges();
        }



        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            EnsureAudit();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }



        public async Task<IEnumerable<T>> QueryAsync<T>(String text, object parametres = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, text, parametres, timeout, type, CancellationToken.None);
            var connection = this.Database.GetDbConnection();
            return await connection.QueryAsync<T>(command.Definition);
        }


        public async Task<T> QueryFirstOrDefaultAsync<T>(string text, object parametres = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, text, parametres, timeout, type, CancellationToken.None);
            var connection = this.Database.GetDbConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(command.Definition);
        }

        public async Task<int> ExecuteAsync(String text, object parametres = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, text, parametres, timeout, type, CancellationToken.None);
            var connection = this.Database.GetDbConnection();
            return await connection.ExecuteAsync(command.Definition);
        }

        public async Task<T> ExecuteScalarAsync<T>(string query, object parameters = null, int? timeout = null, CommandType? type = null)
        {
            using var command = new DapperEFCoreCommand(this, query, parameters, timeout, type, CancellationToken.None);
            var connection = this.Database.GetDbConnection();
            return await connection.ExecuteScalarAsync<T>(command.Definition);
        }


        private void EnsureAudit()
        {
            ChangeTracker.DetectChanges();

            var userEmail = _currentRequestUserService.GetCurrentUserEmail();
            var timestamp = DateTime.UtcNow;

            var auditLogsToCreate = new List<AudiLog>();

            // Iterar sobre todas las entradas rastreadas por Entity Framework Core
            foreach (var entry in ChangeTracker.Entries())
            {
                // Ignorar la propia tabla de auditoría para evitar recursión
                // Ignorar entidades que no han cambiado o están desvinculadas
                if (entry.Entity is AudiLog || // Importante: usa el nombre de tu clase de entidad AuditLog
                    entry.State == EntityState.Detached ||
                    entry.State == EntityState.Unchanged)
                {
                    continue;
                }

              

                var auditEntry = new AudiLog
                {
                    UserEmail = userEmail,
                    EntityName = entry.Metadata.DisplayName(), // Nombre de la clase de la entidad
                    Timestamp = timestamp,
                    InformationType = "Info", // Para operaciones exitosas
                    Message = ""
                };

                switch (entry.State)
                {
                    case EntityState.Added:
                        // Lógica de IAuditableEntity para Added
                        if (auditableEntity != null)
                        {
                            auditableEntity.CreatedDate = timestamp;
                            auditableEntity.IsDeleted = false;
                            // Aseguramos que EF Core no intente marcar estas propiedades como "cambiadas"
                            // si ya las hemos establecido aquí o no provienen de un DTO
                            entry.Property(nameof(IAuditableEntity.CreatedDate)).IsModified = false;
                            entry.Property(nameof(IAuditableEntity.IsDeleted)).IsModified = false;
                        }

                        auditEntry.ActionType = "Insert";
                        auditEntry.NewValues = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                        auditEntry.Message = $"Se insertó '{auditEntry.EntityName}' por '{userEmail}'.";
                        break;

                    case EntityState.Modified:
                        var oldValues = new Dictionary<string, object>();
                        var newValues = new Dictionary<string, object>();
                        var changedColumns = new List<string>();

                        bool isDeletedChangedToTrue = false;  // Indica borrado lógico
                        bool isDeletedChangedToFalse = false; // Indica restauración lógica

                        // Recorrer las propiedades para encontrar cuáles cambiaron y sus valores
                        foreach (var property in entry.Properties)
                        {
                            // Solo procesar propiedades que han sido modificadas
                            if (property.IsModified)
                            {
                                // Detectar el cambio en IsDeleted específicamente para entidades auditables
                                if (auditableEntity != null && property.Metadata.Name == nameof(IAuditableEntity.IsDeleted))
                                {
                                    if (property.OriginalValue is bool originalBool && property.CurrentValue is bool currentBool)
                                    {
                                        if (originalBool == false && currentBool == true)
                                        {
                                            isDeletedChangedToTrue = true;
                                            // Actualizar DeletedDate para borrado lógico
                                            auditableEntity.DeletedDate = timestamp;
                                        }
                                        else if (originalBool == true && currentBool == false)
                                        {
                                            isDeletedChangedToFalse = true;
                                            // Limpiar DeletedDate si es una restauración lógica (o establecer en null)
                                            auditableEntity.DeletedDate = null;
                                        }
                                    }
                                }
                                // Si UpdatedDate es la propiedad modificada y no es una eliminación/restauración lógica,
                                // la actualizamos al final del bloque para la entidad.
                                else if (auditableEntity != null && property.Metadata.Name == nameof(IAuditableEntity.UpdatedDate))
                                {
                                    // Esta propiedad se actualiza explícitamente en la entidad más adelante si no es borrado/restauración
                                }

                                oldValues[property.Metadata.Name] = property.OriginalValue;
                                newValues[property.Metadata.Name] = property.CurrentValue;
                                changedColumns.Add(property.Metadata.Name);
                            }
                        }

                        // Lógica para actualizar UpdatedDate para modificaciones "normales"
                        if (auditableEntity != null && !isDeletedChangedToTrue && !isDeletedChangedToFalse)
                        {
                            auditableEntity.UpdatedDate = timestamp;
                        }


                        auditEntry.OldValues = JsonSerializer.Serialize(oldValues);
                        auditEntry.NewValues = JsonSerializer.Serialize(newValues);
                        auditEntry.ChangedColumns = JsonSerializer.Serialize(changedColumns);

                        // Determinar el ActionType y el mensaje basado en los cambios detectados
                        if (isDeletedChangedToTrue)
                        {
                            auditEntry.ActionType = "Delete (Logical)";
                            auditEntry.Message = $"Se eliminó lógicamente '{auditEntry.EntityName}' por '{userEmail}'.";
                        }
                        else if (isDeletedChangedToFalse)
                        {
                            auditEntry.ActionType = "Restore (Logical)";
                            auditEntry.Message = $"Se restauró lógicamente '{auditEntry.EntityName}' por '{userEmail}'.";
                        }
                        else
                        {
                            auditEntry.ActionType = "Update";
                            auditEntry.Message = $"Se actualizó '{auditEntry.EntityName}' por '{userEmail}'. Columnas: {string.Join(", ", changedColumns)}.";
                        }
                        break;

                    case EntityState.Deleted:
                        // Este 'case' se ejecutará ahora para cualquier entidad que se marque como EntityState.Deleted.
                        // Si tu repositorio maneja el borrado lógico correctamente (cambiando a Modified y IsDeleted=true),
                        // entonces las IAuditableEntity no deberían llegar aquí con EntityState.Deleted.
                        // Si llegan aquí, se tratarán como eliminación física.
                        if (auditableEntity != null)
                        {
                            // Si la entidad se va a eliminar físicamente, asegura que sus campos de auditoría reflejen esto.
                            auditableEntity.IsDeleted = true;
                            auditableEntity.DeletedDate = timestamp;
                            // Asegura que EF Core no intente guardar estos cambios ya que la entidad se va a eliminar
                            entry.Property(nameof(IAuditableEntity.IsDeleted)).IsModified = false;
                            entry.Property(nameof(IAuditableEntity.DeletedDate)).IsModified = false;
                        }

                        auditEntry.ActionType = "Delete (Physical)";
                        auditEntry.OldValues = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                        auditEntry.Message = $"Se eliminó físicamente '{auditEntry.EntityName}' por '{userEmail}'.";
                        break;
                }
                auditLogsToCreate.Add(auditEntry);
            }

            // Añadir las entradas de auditoría al contexto para que se guarden en la misma transacción
            foreach (var auditLog in auditLogsToCreate)
            {
                AuditLogs.Add(auditLog);
            }
        }



        public readonly struct DapperEFCoreCommand : IDisposable
        {
            public DapperEFCoreCommand(DbContext context, String text, object parametres, int? timeout, CommandType? type, CancellationToken ct)
            {
                var transaction = context.Database.CurrentTransaction?.GetDbTransaction();
                var commandType = type ?? CommandType.Text;
                var commandTimeout = timeout ?? context.Database.GetCommandTimeout() ?? 30;
                Definition = new CommandDefinition(
                    text, parametres, transaction, commandTimeout, commandType, cancellationToken: ct
                    );


            }
            public CommandDefinition Definition { get; }

            public void Dispose()
            {
            }

        }

  
    }
}
