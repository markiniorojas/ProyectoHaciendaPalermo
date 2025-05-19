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
    class FormModuleRelations : IEntityTypeConfiguration<FormModule>
    {
        public void Configure(EntityTypeBuilder<FormModule> builder)
        {
            // Nombre de la tabla
            builder.ToTable("formModule");

            // Clave primaria
            builder.HasKey(fm => fm.Id);

            // Relación: FormModule -> Form (muchos a uno)
            builder.HasOne(fm => fm.Form)
                   .WithMany(f => f.FormModules)
                   .HasForeignKey(fm => fm.FormId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Relación: FormModule -> Module (muchos a uno)
            builder.HasOne(fm => fm.Module)
                   .WithMany(m => m.FormModule)
                   .HasForeignKey(fm => fm.ModuleId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Propiedades
            builder.Property(fm => fm.IsDeleted)
                   .IsRequired();

        }
    }
}
