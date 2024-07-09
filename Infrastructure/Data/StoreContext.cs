using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {

        }

        public DbSet<Product> Products {get; set;}
        public DbSet<ProductType> ProductTypes {get; set;}
        public DbSet<ProductBrand> ProductBrands {get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}

// Terminal commands for Track Record
//1. dotnet ef database drop -p Infrastructure -s API // this drops the database //removes the igrations
//2. dotnet ef migrations remove -p Infrastructure -s API // this removes the migrations // emoties the migrations folder
//3. dotnet ef migrations add InitialCreate -p Infrastructure -s API -o Data/Migrations // this creates migrations in Infra project in data/migrations
