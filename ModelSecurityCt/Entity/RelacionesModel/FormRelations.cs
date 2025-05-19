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
    public class FormRelations : IEntityTypeConfiguration<Form>
    {
        public void Configure(EntityTypeBuilder<Form> builder)
        {
            // Configuración de la tabla
            builder.ToTable("form");

            // Clave primaria
            builder.HasKey(f => f.Id);

            // Relación: Form -> RolFormPermission (uno a muchos)
            builder.HasMany(f => f.RolFormPermission)
                .WithOne(rfp => rfp.Form)
                .HasForeignKey(rfp => rfp.FormId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relacion: Form -> FormModule (uno a muchos)
            builder.HasMany(f => f.FormModules)
                .WithOne(fm => fm.Form)
                .HasForeignKey(fm => fm.FormId)
                .OnDelete(DeleteBehavior.Cascade);

            // Propiedades
            builder.Property(f => f.Name)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(f => f.Description)
                .HasMaxLength(300);

            builder.Property(f => f.IsDeleted)
                .IsRequired();

        }
    }
}
