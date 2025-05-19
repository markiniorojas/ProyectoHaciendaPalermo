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
    public class RolRelations : IEntityTypeConfiguration<Rol>
    {
        public void Configure(EntityTypeBuilder<Rol> builder)
        {
            builder.ToTable("rol");

            // Clave primaria
            builder.HasKey(r => r.Id);

            // Relación: Rol -> RolUser (uno a muchos)
            builder.HasMany(r => r.RolUsers)
                .WithOne(ru => ru.Rol)
                .HasForeignKey(ru => ru.RolId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relación: Rol -> RolFormPermission (uno a muchos)
            builder.HasMany(r => r.RolFormPermission)
                .WithOne(rfp => rfp.Rol)
                .HasForeignKey(rfp => rfp.RolId)
                .OnDelete(DeleteBehavior.Cascade);

            // Propiedades
            builder.Property(r => r.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(r => r.Description)
                .IsRequired()
                .HasMaxLength(255);

            builder.Property(r => r.IsDeleted)
                .HasDefaultValue(false);
        }
    }
}
