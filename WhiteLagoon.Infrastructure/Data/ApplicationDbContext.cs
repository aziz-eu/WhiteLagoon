using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhiteLagoon.Domain.Entities;

namespace WhiteLagoon.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) 
        {
            
        }
        public DbSet<Villa> Villas { get; set; }
        public DbSet<VillaNumber> VillaNumbers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Villa>().HasData(
                new Villa
                {
                    Id = 1,
                    Name = "Ruposhi Bangla",
                    Description = "Ruposhi Bangla offers an exceptional stay in Dhaka, Bangladesh",
                    Price = 100,
                    Sqft=400,
                    Occupancy=2,
                    ImageUrl= "https://dummyimage.com/300",

                },
                 new Villa
                 {
                     Id = 2,
                     Name = "White Hall",
                     Description = "White Hall offers an exceptional stay in Dhaka, Bangladesh",
                     Price = 90,
                     Sqft = 500,
                     Occupancy = 4,
                     ImageUrl = "https://dummyimage.com/300",

                 }
                );

            modelBuilder.Entity<VillaNumber>().HasData(
                new VillaNumber
                {
                    Villa_Number = 100,
                    VillaId = 1

                },
                 new VillaNumber
                 {
                     Villa_Number = 101,
                     VillaId = 1

                 },
                  new VillaNumber
                  {
                      Villa_Number = 200,
                      VillaId = 2

                  },
                 new VillaNumber
                 {
                     Villa_Number = 201,
                     VillaId = 2

                 }

                ) ;
        }
    }
}
