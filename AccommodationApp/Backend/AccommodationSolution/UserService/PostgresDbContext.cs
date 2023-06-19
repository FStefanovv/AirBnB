using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Users.Model;

namespace Users
{
    public class PostgresDbContext : DbContext
    {
        public PostgresDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
            modelBuilder.Entity<User>()
                .ToTable("Users");

            modelBuilder.Entity<User>()
                .HasData(
                new User {
                    Id = "1",
                    FirstName = "Ime",
                    LastName = "Prezime",
                    Username = "imeprezime",
                    Email = "imeprezime@gmail.com",
                    Password = "sifra123",
                    Role = "HOST"
                },
                new User
                {
                    Id = "2",
                    FirstName = "Imenko",
                    LastName = "Prezimenic",
                    Username = "imenkoprezimenko",
                    Email = "imenkoprezimenko@gmail.com",
                    Password = "sifra123",
                    Role = "HOST"
                }
                );

            modelBuilder.Entity<User>().OwnsOne(u => u.Address);


            base.OnModelCreating(modelBuilder);
        }

        public DbSet<User> Users { get; set; }
    }
}
