using Microsoft.EntityFrameworkCore;
using ParkyAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<NationalPark> NationalParks { get; set; }
        public DbSet<Trail> Trails { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NationalPark>().HasData(
                new NationalPark
                {
                    Id = 1,
                    Name = "NP",
                    State = "IL",
                    Created = DateTime.Now,
                    Established = DateTime.Now
                },
                new NationalPark
                {
                    Id = 2,
                    Name = "NPTest",
                    State = "TS",
                    Created = DateTime.Now,
                    Established = DateTime.Now
                });

            modelBuilder.Entity<User>().HasData(
                new User()
                {
                    Id = 1,
                    Username = "h4mmm1d",
                    Password = "123456aA",
                    Role = "Admin"
                });
        }
    }
}
