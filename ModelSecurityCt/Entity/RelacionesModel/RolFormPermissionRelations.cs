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
    class RolFormPermissionRelations : IEntityTypeConfiguration<RolFormPermission>
    {
        public void Configure(EntityTypeBuilder<RolFormPermission> builder)
        {
            // Nombre de la tabla
            builder.ToTable("rolFormPermission");

            // Clave primaria
            builder.HasKey(rfp => rfp.Id);

            // Relación: RolFormPermission -> Rol (muchos a uno)
            builder.HasOne(rfp => rfp.Rol)
                   .WithMany(r => r.RolFormPermission)
                   .HasForeignKey(rfp => rfp.RolId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relación: RolFormPermission -> Form (muchos a uno)
            builder.HasOne(rfp => rfp.Form)
                   .WithMany(f => f.RolFormPermission)
                   .HasForeignKey(rfp => rfp.FormId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relación: RolFormPermission -> Permission (muchos a uno)
            builder.HasOne(rfp => rfp.Permission)
                   .WithMany(p => p.RolFormPermission)
                   .HasForeignKey(rfp => rfp.PermissionId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Propiedades
            builder.Property(rfp => rfp.IsDeleted)
                   .IsRequired();
        }
    }
}
