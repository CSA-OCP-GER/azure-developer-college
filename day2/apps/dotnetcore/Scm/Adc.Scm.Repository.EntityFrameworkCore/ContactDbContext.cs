using Adc.Scm.DomainObjects;
using Microsoft.EntityFrameworkCore;
using System;

namespace Adc.Scm.Repository.EntityFrameworkCore
{
    public class ContactDbContext : DbContext
    {
        public ContactDbContext(DbContextOptions options)
            : base(options)
        {

        }
        public DbSet<Contact> Contacts { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            var contact = builder.Entity<Contact>();
            contact
                .ToTable("contacts")
                .HasKey(c => c.Id)
                .HasName("id");
            contact
                .Property(c => c.Firstname)
                .HasColumnName("fristname")
                .IsUnicode()
                .IsRequired();
            contact
                .Property(c => c.Lastname)
                .HasColumnName("lastname")
                .IsUnicode()
                .IsRequired();
            contact
                .Property(c => c.Email)
                .HasColumnName("email")
                .IsUnicode()
                .IsRequired();
            contact
                .Property(c => c.AvatarLocation)
                .HasColumnName("avatarlocation")
                .IsUnicode();

            base.OnModelCreating(builder);
        }
    }
}
