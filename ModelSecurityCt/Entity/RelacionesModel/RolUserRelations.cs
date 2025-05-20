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
    public class RolUserRelations : IEntityTypeConfiguration<RolUser>
    {
        public void Configure(EntityTypeBuilder<RolUser> builder)
        {
            ///<Summary>
            ///Esqumas de seguridad:
            ///<Summary>
            builder.ToTable("rolUser", schema: "Seguridad");

            // Clave primaria
            builder.HasKey(ru => ru.Id);

            // Relación: RolUser -> Rol (muchos a uno)
            builder.HasOne(ru => ru.Rol)
                   .WithMany(r => r.RolUsers)
                   .HasForeignKey(ru => ru.RolId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Relación: RolUser -> User (muchos a uno)
            builder.HasOne(ru => ru.User)
                   .WithMany(u => u.RolUsers)
                   .HasForeignKey(ru => ru.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            // Propiedades adicionales
            builder.Property(ru => ru.Email)
                   .HasMaxLength(200);

            builder.Property(ru => ru.IsDeleted)
                .HasDefaultValue(false);

        }
    }
}
