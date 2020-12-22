using System.Collections.Generic;
using dotnet_kafka_integration.Model;
using Microsoft.EntityFrameworkCore;
namespace dotnet_kafka_integration
{

    public class OrderDbContext : DbContext
    {

        public DbSet<OrderRequest> OrderRequest { get; set; }
        public DbSet<Product> Product { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
         => options.UseSqlite("Data Source=blogging.db");
    }

}