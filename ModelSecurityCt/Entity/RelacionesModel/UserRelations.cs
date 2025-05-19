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
    public class UserRelations : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("user");

            //Clave primaria
            builder.HasKey(u => u.Id);

            //Relacion Con person
            builder.HasOne(u => u.Person)
                .WithMany(p => p.Users)
                .HasForeignKey(u => u.PersonId)
                .OnDelete(DeleteBehavior.Restrict);

            //rELACION con RolUser
            builder.HasMany(u => u.RolUsers)
                .WithOne(r => r.User)
                .HasForeignKey(ru => ru.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            //Propiedades
            builder.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(u => u.Password)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(u => u.Active)
                   .HasDefaultValue(true);

            builder.Property(u => u.IsDeleted)
                   .HasDefaultValue(false);

        }
    }
}
