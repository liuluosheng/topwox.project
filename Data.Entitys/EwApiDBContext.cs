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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }


    }
}

