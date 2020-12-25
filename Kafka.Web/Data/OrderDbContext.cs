using System.Collections.Generic;
using Dotnet.Kafka.Integration.Model;
using Microsoft.EntityFrameworkCore;
namespace Dotnet.Kafka.Integration
{

    public class OrderDbContext : DbContext
    {

        public DbSet<OrderRequest> OrderRequest { get; set; }
        public DbSet<Product> Product { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
         => options.UseSqlite("Data Source=blogging.db");
    }

}