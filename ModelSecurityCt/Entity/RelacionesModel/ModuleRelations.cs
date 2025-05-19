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
    class ModuleRelations : IEntityTypeConfiguration<Module>
    {
        public void Configure(EntityTypeBuilder<Module> builder)
        {
            // Nombre de la tabla
            builder.ToTable("module");

            // Clave primaria
            builder.HasKey(m => m.Id);

            // Relación: Module -> FormModule (uno a muchos)
            builder.HasMany(m => m.FormModule)
                   .WithOne(fm => fm.Module)
                   .HasForeignKey(fm => fm.ModuleId)
                   .OnDelete(DeleteBehavior.Cascade);

            // Propiedades
            builder.Property(m => m.Name)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(m => m.Description)
                   .HasMaxLength(300);

            builder.Property(m => m.IsDeleted)
                   .IsRequired();
        }
    }
}
