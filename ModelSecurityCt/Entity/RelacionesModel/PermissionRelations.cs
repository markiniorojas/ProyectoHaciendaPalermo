using Entity.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.RelacionesModel
{
    public class PermissionRelations : IEntityTypeConfiguration<Permission>
    {
        public void Configure(EntityTypeBuilder<Permission> builder)
        {
            // Configuración de la tabla y implementar el esquema de seguridad
            builder.ToTable("permission", schema: "Seguridad");

            // Clave primaria
            builder.HasKey(p => p.Id);

            // Relación: Permission -> RolFormPermission (uno a muchos)
            builder.HasMany(p => p.RolFormPermission)
                .WithOne(rfp => rfp.Permission)
                .HasForeignKey(rfp => rfp.PermissionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Propiedades
            builder.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            // Añadir índice único para Name (los nombres de permisos suelen ser únicos)
            builder.HasIndex(p => p.Name).IsUnique();

            builder.Property(p => p.Description)
                   .HasMaxLength(300);

            builder.Property(p => p.IsDeleted)
                   .IsRequired();
        }
    }
}
