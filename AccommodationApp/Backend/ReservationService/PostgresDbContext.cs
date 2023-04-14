using Microsoft.EntityFrameworkCore;
using ReservationService.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReservationService
{
    public class PostgresDbContext : DbContext
    {
        public PostgresDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ReservationRequest> Requests { get; set; }
        public DbSet<Reservation> Reservations { get; set; }


        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
          modelBuilder.Entity<ReservationRequest>()
              .ToTable("Requests");

          modelBuilder.Entity<ReservationRequest>()
              .HasData(
              new ReservationRequest
              {
                  Id = "1",
                  From = new DateTime(2023, 5, 10),
                  To = new DateTime(2023, 5, 15),
                  UserId = "f6b022d5-15bf-44d9-9217-e542e68585a0",
                  AccommodationId = "643455b8941a49684fb8cfc1",
                  NumberOfGuests = 12,
                  Status = Enums.RequestStatus.PENDING
              },
              new ReservationRequest
              {
                  Id = "2",
                  From = new DateTime(2023, 5, 20),
                  To = new DateTime(2023, 5, 28),
                  UserId = "7bf59c03-17a6-49a9-92f1-4c251b0d0bd8",
                  AccommodationId = "643455b8941a49684fb8cfc1",
                  NumberOfGuests = 10,
                  Status = Enums.RequestStatus.PENDING
              }
              );

            modelBuilder.Entity<Reservation>()
               .ToTable("Reservations");

            modelBuilder.Entity<Reservation>()
                .HasData(
                    new Reservation
                    {
                        Id = "1",
                        From = new DateTime(2023, 5, 20),
                        To = new DateTime(2023, 5, 28),
                        UserId = "7bf59c03-17a6-49a9-92f1-4c251b0d0bd8",
                        AccommodationId = "643455b8941a49684fb8cfc1",
                        NumberOfGuests = 10,
                        Status = Enums.ReservationStatus.ACTIVE
                    }    
                );

            base.OnModelCreating(modelBuilder);
        }

    }
}
