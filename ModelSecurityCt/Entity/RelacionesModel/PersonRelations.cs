using Entity.Model;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.RelacionesModel
{
    public class RelacionPerson : IEntityTypeConfiguration<Person>
    {
        public void Configure(EntityTypeBuilder<Person> builder)
        {
            ///<Summary>
            ///Implementacion del esquema de seguridad:
            ///<Summary>

            builder.ToTable("person", schema: "Seguridad");


            // Clave primaria
            builder.HasKey(p => p.Id);

            // Relación: Person -> Users (uno a muchos)
            builder.HasMany(p => p.Users)
                   .WithOne(u => u.Person)
                   .HasForeignKey(u => u.PersonId)
                   .OnDelete(DeleteBehavior.Restrict); // evita eliminar persona si hay usuarios relacionados

            // Validaciones adicionales si deseas
            builder.Property(p => p.FirstName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.LastName)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(p => p.Document)
                   .IsRequired()
                   .HasMaxLength(50);

            builder.Property(p => p.IsDeleted)
                   .HasDefaultValue(false);
        }
    }
}
