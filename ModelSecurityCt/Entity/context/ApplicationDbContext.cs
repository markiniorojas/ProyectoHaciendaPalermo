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

            string userEmail = _currentRequestUserService.GetCurrentUserEmail();
            DateTime timestamp = DateTime.UtcNow;

            List<AudiLog> auditLogsToCreate = new List<AudiLog>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AudiLog || // Ignorar la propia tabla de auditoría
                    entry.State == EntityState.Detached ||
                    entry.State == EntityState.Unchanged)
                {
                    continue;
                }

                // Eliminado: IAuditableEntity auditableEntity = entry.Entity as IAuditableEntity;

                AudiLog auditEntry = new AudiLog
                {
                    // UserId = userId, // <--- Si eliminaste UserId de AuditLog, remueve esta línea
                    Email = userEmail,
                    NameTable = entry.Metadata.DisplayName(),
                    Timestamp = timestamp,
                    InformationType = "Info",
                    Message = ""
                };

                switch (entry.State)
                {
                    case EntityState.Added:
                        auditEntry.ActionType = "Insert";
                        auditEntry.NewValue = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                        auditEntry.Message = $"Se insertó '{auditEntry.NameTable}' por '{userEmail}'.";
                        break;

                    case EntityState.Modified:
                        Dictionary<string, object> oldValues = new Dictionary<string, object>();
                        Dictionary<string, object> newValues = new Dictionary<string, object>();
                        List<string> changedColumns = new List<string>();

                        bool isDeletedPropertyExists = false;
                        bool originalIsDeleted = false;
                        bool currentIsDeleted = false;

                        foreach (var property in entry.Properties)
                        {
                            if (property.IsModified)
                            {
                                // Detectar el cambio en una propiedad "IsDeleted" si existe
                                if (property.Metadata.Name == "IsDeleted" && property.OriginalValue is bool originalBool && property.CurrentValue is bool currentBool)
                                {
                                    isDeletedPropertyExists = true;
                                    originalIsDeleted = originalBool;
                                    currentIsDeleted = currentBool;
                                }

                                oldValues[property.Metadata.Name] = property.OriginalValue;
                                newValues[property.Metadata.Name] = property.CurrentValue;
                                changedColumns.Add(property.Metadata.Name);
                            }
                        }

                        auditEntry.OldValue = JsonSerializer.Serialize(oldValues);
                        auditEntry.NewValue = JsonSerializer.Serialize(newValues);
                        auditEntry.ChangedColumns= JsonSerializer.Serialize(changedColumns);

                        if (isDeletedPropertyExists && originalIsDeleted == false && currentIsDeleted == true)
                        {
                            auditEntry.ActionType = "Delete (Logical)";
                            auditEntry.Message = $"Se eliminó lógicamente '{auditEntry.NameTable}' por '{userEmail}'.";
                        }
                        else if (isDeletedPropertyExists && originalIsDeleted == true && currentIsDeleted == false)
                        {
                            auditEntry.ActionType = "Restore (Logical)";
                            auditEntry.Message = $"Se restauró lógicamente '{auditEntry.NameTable}' por '{userEmail}'.";
                        }
                        else
                        {
                            auditEntry.ActionType = "Update";
                            auditEntry.Message = $"Se actualizó '{auditEntry.NameTable}' por '{userEmail}'. Columnas: {string.Join(", ", changedColumns)}.";
                        }
                        break;

                    case EntityState.Deleted:
                        auditEntry.ActionType = "Delete (Physical)";
                        auditEntry.OldValue = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                        auditEntry.Message = $"Se eliminó físicamente '{auditEntry.NameTable}' por '{userEmail}'.";
                        break;
                }
                auditLogsToCreate.Add(auditEntry);
            }

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
