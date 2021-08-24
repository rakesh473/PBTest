using Microsoft.EntityFrameworkCore;
using PBSqlite.Models;

namespace PBSqlite.Services
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Table> Table { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=PBSqlite.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Table>().ToTable("Table");

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    UserName = "admin",
                    Password = "admin"
                });
            modelBuilder.Entity<Table>().HasData(
                new Table
                {
                    Id = 1,
                    TableName = "08202021",
                    PlayersData =
                        "[{ \"Id\": 1, \"Name\": \"TEST\", \"BuyIns\": 0, \"Close\": 0.0, \"Total\": -5.0 }, { \"Id\": 2, \"Name\": \"TEST\", \"BuyIns\": 0, \"Close\": 0.0, \"Total\": -5.0 }, { \"Id\": 3, \"Name\": \"TEST\", \"BuyIns\": 0, \"Close\": 0.0, \"Total\": -5.0 }, { \"Id\": 4, \"Name\": \"TEST\", \"BuyIns\": 0, \"Close\": 0.0, \"Total\": -5.0 }, { \"Id\": 5, \"Name\": \"TEST\", \"BuyIns\": 0, \"Close\": 0.0, \"Total\": -5.0 }, { \"Id\": 6, \"Name\": \"TEST\", \"BuyIns\": 0, \"Close\": 0.0, \"Total\": -5.0 }, { \"Id\": 7, \"Name\": \"TEST\", \"BuyIns\": 0, \"Close\": 0.0, \"Total\": -5.0 }, { \"Id\": 8, \"Name\": \"TEST\", \"BuyIns\": 0, \"Close\": 0.0, \"Total\": -5.0 }, { \"Id\": 9, \"Name\": \"TEST\", \"BuyIns\": 0, \"Close\": 0.0, \"Total\": -5.0 }]",
                    TotalBuyIns = 0,
                    TotalGain = 0,
                    TotalLoss = 0,
                    TotalTable = 0
                });
        }
    }
}