using Data.Entitys;
using Microsoft.EntityFrameworkCore;
using System;

namespace Data
{
    public class EwApiDBContext : DbContext
    {
        public EwApiDBContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<User> Users { get; set; }

        public DbSet<Employees> Employees { get; set; }

        public DbSet<Product> Products { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


    }
}

